/*
 * Copyright 2013 XPFriend Community.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using XPFriend.Fixture.Cast.Temp;
using XPFriend.Fixture.Staff;
using XPFriend.Fixture.Toolkit;
using XPFriend.Junk;
using XPFriend.Properties;

namespace XPFriend.Fixture
{
    /// <summary>
    /// FixtureBook 操作ツール。
    /// </summary>
    public class FixtureBook
    {
        internal class FixtureInfo
        {
            public Type TestClass { get; set; }
            public string FilePath { get; set; }
            public string SheetName { get; set; }
            public string TestCaseName { get; set; }
        }

        internal const string NameSeparatorKey = "FixtureBook.nameSeparator";
        internal const string DefaultNameSeparator = "__";
        private static readonly string[] SpecialMethodSeparator = new string[] { "<", ">" };
        private static string[] nameSeparator;

        static FixtureBook()
        {
            Resi.Add(Resources.ResourceManager);
            InitNameSeparator();
        }

        internal static void InitNameSeparator()
        {
            string separator = Config.Get(NameSeparatorKey, DefaultNameSeparator);
            nameSeparator = new string[] { separator };
        }

        private static string[] GetNamesByClassAndMethodName(Type type, MemberInfo method)
        {
            return new string[] { type.Name, method.Name };
        }

        private static string[] GetNamesByAttribute(Type type, MemberInfo method)
        {
            FixtureAttribute attribute = (FixtureAttribute)Attribute.GetCustomAttribute(method, typeof(FixtureAttribute));
            if (attribute == null)
            {
                return null;
            }
            string category = attribute.Category;
            if (category == null)
            {
                category = type.Name;
            }
            return new string[] { category, attribute.Description };
        }

        private static string[] GetNamesByMethodName(string name)
        {
            if(name.StartsWith("<"))
            {
                name = name.Split(SpecialMethodSeparator, StringSplitOptions.RemoveEmptyEntries)[0];
            }
            return name.Split(nameSeparator, StringSplitOptions.RemoveEmptyEntries);
        }

        private static string GetFilePath(StackFrame stackFrame, Type type, MemberInfo method)
        {
            Attribute attribute = Attribute.GetCustomAttribute(method, typeof(FixtureBookAttribute));
            if (attribute == null)
            {
                attribute = Attribute.GetCustomAttribute(type, typeof(FixtureBookAttribute));
            }

            if (attribute != null)
            {
                string path = ((FixtureBookAttribute)attribute).Name;
                if (path != null)
                {
                    string tmp = PathUtil.EditFilePath(path);
                    if (File.Exists(tmp))
                    {
                        return tmp;
                    }

                    if (path.Equals(Path.GetFileName(path)))
                    {
                        string directoryName = Path.GetDirectoryName(stackFrame.GetFileName());
                        path = Path.Combine(directoryName, path);
                    }
                    return PathUtil.EditFilePath(path);
                }
            }

            return PathUtil.EditFilePath(GetDefaultFilePath(stackFrame));
        }

        private static string GetDefaultFilePath(StackFrame frame)
        {
            string testClassFileName = frame.GetFileName();
            return Path.Combine(
                Path.GetDirectoryName(testClassFileName),
                Path.GetFileNameWithoutExtension(testClassFileName) + ".xlsx");
        }

        private Type testClass;
        private Book book;
        private Sheet sheet;
        private Case testCase;

        /// <summary>
        /// FixtureBook を作成する。
        /// </summary>
        public FixtureBook() : this(false) { }

        private FixtureBook(bool initialize)
        {
            if (initialize)
            {
                Initialize();
            }
        }

        private void Initialize() 
        {
            Initialize(StackFrameFinder.FindCaller(typeof(FixtureBook)));
        }

        private void Initialize(List<StackFrame> stackFrames)
        {
            for(int i = 0; i < stackFrames.Count; i++) 
            {
                FixtureInfo fixtureInfo = GetFixtureInfo(stackFrames[i], false);
                if(fixtureInfo != null) 
                {
                    Initialize(fixtureInfo);
                    return;
                }
            }
            Initialize(GetFixtureInfo(stackFrames[0], true));
        }

        private static FixtureInfo GetFixtureInfo(StackFrame stackFrame, Boolean force) 
        {
            MemberInfo method = stackFrame.GetMethod();
            Type type = method.ReflectedType;
            String[] name = GetNamesByAttribute(type, method);
            if(name != null) 
            {
                return CreateFixtureInfo(stackFrame, method, type, name);
            }

            name = GetNamesByMethodName(method.Name);
            if (name.Length >= 2)
            {
                return CreateFixtureInfo(stackFrame, method, type, name);
            }

            if(force)
            {
                name = GetNamesByClassAndMethodName(type, method);
                return CreateFixtureInfo(stackFrame, method, type, name);
            }
            return null;
        }

        private static FixtureInfo CreateFixtureInfo(StackFrame stackFrame, MemberInfo method, Type type, String[] name)
        {
            string filePath = GetFilePath(stackFrame, type, method);
            return new FixtureInfo{TestClass = type, FilePath = filePath, SheetName = name[0], TestCaseName = name[1]};
        }

        private void Initialize(FixtureInfo fixtureInfo)
        {
            testClass = fixtureInfo.TestClass;
            book = Book.GetInstance(fixtureInfo.TestClass, fixtureInfo.FilePath);
            sheet = book.GetSheet(fixtureInfo.SheetName);
            testCase = sheet.GetCase(fixtureInfo.TestCaseName);
            Loggi.Debug("FixtureBook : Case : " + testCase);
        }

        /// <summary>
        /// 「B.テストデータクリア条件」と「C.テストデータ」を用いてデータベースに対する更新を行う。
        /// </summary>
        public void Setup()
        {
            InitializeIfNotYet();
            testCase.Setup();
        }

        private void InitializeIfNotYet()
        {
            if (testCase == null)
            {
                Initialize();
            }
        }

        /// <summary>
        /// 「D.パラメタ」に記述されたデータを用いてオブジェクトを作成する。
        /// </summary>
        /// <typeparam name="T">作成するオブジェクトのクラス</typeparam>
        /// <param name="name">テーブル定義名</param>
        /// <returns>作成されたオブジェクト</returns>
        public T GetObject<T>(params string[] name)
        {
            InitializeIfNotYet();
            return testCase.GetObject<T>(name);
        }

        /// <summary>
        /// 「D.パラメタ」に記述されたデータを用いてオブジェクトのリストを作成する。
        /// </summary>
        /// <typeparam name="T">作成するリスト要素オブジェクトクラス</typeparam>
        /// <returns>作成されたリスト</returns>
        public List<T> GetList<T>()
        {
            return GetList<T>(null);
        }

        /// <summary>
        /// 「D.パラメタ」に記述されたデータを用いてオブジェクトのリストを作成する。
        /// </summary>
        /// <typeparam name="T">作成するリスト要素オブジェクトクラス</typeparam>
        /// <param name="name">テーブル定義名</param>
        /// <returns>作成されたリスト</returns>
        public List<T> GetList<T>(string name)
        {
            InitializeIfNotYet();
            return testCase.GetList<T>(name);
        }

        /// <summary>
        /// 「D.パラメタ」に記述されたデータを用いてオブジェクトの配列を作成する。
        /// </summary>
        /// <typeparam name="T">作成する配列要素オブジェクトクラス</typeparam>
        /// <returns>作成された配列</returns>
        public T[] GetArray<T>()
        {
            return GetArray<T>(null);
        }

        /// <summary>
        /// 「D.パラメタ」に記述されたデータを用いてオブジェクトの配列を作成する。
        /// </summary>
        /// <typeparam name="T">作成する配列要素オブジェクトクラス</typeparam>
        /// <param name="name">テーブル定義名</param>
        /// <returns>作成された配列</returns>
        public T[] GetArray<T>(string name)
        {
            InitializeIfNotYet();
            return testCase.GetArray<T>(name);
        }

        /// <summary>
        /// 指定されたオブジェクトが「E.取得データ」に記述された予想結果と適合するかどうかを調べる。
        /// 予想結果と適合しない場合は例外がスローされる。
        /// </summary>
        /// <param name="obj">調べるオブジェクト</param>
        /// <param name="name">テーブル定義名</param>
        public void Validate(object obj, params string[] name)
        {
            InitializeIfNotYet();
            testCase.Validate(obj, name);
        }

        /// <summary>
        /// 指定された処理で発生した例外が「E.取得データ」に記述された予想結果と適合することを調べる。
        /// </summary>
        /// <typeparam name="TException">発生が予想される例外</typeparam>
        /// <param name="action">テスト対象処理</param>
        public void Validate<TException>(Action action) where TException : Exception
        {
            Validate<TException>(action, null);
        }

        /// <summary>
        /// 指定された処理で発生した例外が「E.取得データ」に記述された予想結果と適合することを調べる。
        /// </summary>
        /// <typeparam name="TException">発生が予想される例外</typeparam>
        /// <param name="action">テスト対象処理</param>
        /// <param name="name">テーブル定義名</param>
        public void Validate<TException>(Action action, string name) where TException : Exception
        {
            InitializeIfNotYet();
            testCase.Validate<TException>(action, name);
        }

        /// <summary>
        /// 現在のデータベース状態が「F.更新後データ」に記述された予想結果と適合するかどうかを調べる。
        /// 予想結果と適合しない場合は例外がスローされる。
        /// </summary>
        public void ValidateStorage()
        {
            InitializeIfNotYet();
            testCase.ValidateStorage();
        }


        /// <summary>
        /// 「B.テストデータクリア条件」と「C.テストデータ」でデータベーステーブルの登録を行い、
        /// 「D.パラメタ」に定義されたオブジェクトを引数にしてテスト対象メソッドを実行し、
        /// 「F.更新後データ」にテーブル定義があれば データベースの値検証を行う。
        /// 
        /// テスト対象メソッドは、テストクラス名およびテストカテゴリ（シート）名からテスト対象クラスおよびメソッドを類推して実行する。
        /// 例えば、テストクラス名が 「ExampleTest」で、テストカテゴリ（シート）名が「Execute」の場合、
        /// Example クラスの Execute メソッドが実行される。
        /// </summary>
        /// <returns>FixtureBook のインスタンス</returns>
        public static FixtureBook Expect()
        {
            return Expect(null, null);
        }

        /// <summary>
        /// 「B.テストデータクリア条件」と「C.テストデータ」でデータベーステーブルの登録を行い、
        /// 「D.パラメタ」に定義されたオブジェクトを引数にしてテスト対象メソッドを実行し、
        /// 「F.更新後データ」にテーブル定義があれば データベースの値検証を行う。
        /// </summary>
        /// <param name="targetClass">テスト対象クラス</param>
        /// <param name="targetMethod">テスト対象メソッド</param>
        /// <param name="targetMethodParameter">テスト対象メソッドのパラメタ</param>
        /// <returns>FixtureBook のインスタンス</returns>
        public static FixtureBook Expect(Type targetClass, string targetMethod, params Type[] targetMethodParameter)
        {
            FixtureBook fixtureBook = new FixtureBook(true);
            if (targetClass == null)
            {
                targetClass = fixtureBook.GetDefaultTargetClass();
            }

            if (targetMethod == null)
            {
                targetMethod = fixtureBook.GetDefaultTargetMethod(targetClass);
            }
            fixtureBook.testCase.Expect(targetClass, targetMethod, targetMethodParameter);
            return fixtureBook;
        }

        private string GetDefaultTargetMethod(Type targetClass)
        {
            string methodName = sheet.Name;
            MethodInfo method = MethodFinder.FindMethod(targetClass, methodName);
            if (method == null)
            {
                throw new ConfigException("M_Fixture_FixtureBook_GetDefaultTargetMethod", methodName, targetClass.FullName, testCase);
            }
            return methodName;
        }

        private Type GetDefaultTargetClass()
        {
            string targetClassName = GetTargetClassName();
            Type targetClass = TypeConverter.ToType(targetClassName, null);
            if (targetClass == typeof(object))
            {
                throw new ConfigException("M_Fixture_FixtureBook_GetDefaultTargetClass", targetClassName, testCase);
            }
            return targetClass;
        }

        private string GetTargetClassName()
        {
            string testClassName = testClass.FullName;
            if (testClassName.EndsWith("Test"))
            {
                return testClassName.Substring(0, testClassName.Length - 4);
            }
            throw new ConfigException("M_Fixture_FixtureBook_GetDefaultTargetClassName", testClassName, testCase);
        }

        /// <summary>
        /// Setup を行い、action を実行し、ValidateStorage を行う。
        /// ただし、「F.更新後データ」にテーブル定義がない場合は ValidateStorage の実行は行われない。
        /// </summary>
        /// <param name="action">テスト対象処理</param>
        /// <returns>FixtureBookのインスタンス</returns>
        public static FixtureBook Expect(Action action)
        {
            FixtureBook fixtureBook = new FixtureBook(true);
            fixtureBook.testCase.Expect(action);
            return fixtureBook;
        }

        /// <summary>
        /// GetObject / GetList / GetArray で取得したオブジェクトを引数にして action を実行し、
        /// ValidateStorage の実行を行う。
        /// GetObject / GetList / GetArray で取得するオブジェクトは
        /// 「D.パラメタ」の最初に定義されているものを使用する。
        /// 「F.更新後データ」にテーブル定義がない場合は ValidateStorage の実行は行われない。
        /// </summary>
        /// <typeparam name="T">actionに渡す引数の型</typeparam>
        /// <param name="action">テスト対象処理</param>
        /// <returns>FixtureBookのインスタンス</returns>
        public static FixtureBook Expect<T>(Action<T> action)
        {
            FixtureBook fixtureBook = new FixtureBook(true);
            fixtureBook.testCase.Expect(action, typeof(T));
            return fixtureBook;
        }

        /// <summary>
        /// GetObject / GetList / GetArray で取得したオブジェクトを引数にして action を実行し、
        /// ValidateStorage の実行を行う。
        /// GetObject / GetList / GetArray で取得するオブジェクトは
        /// それぞれ「D.パラメタ」の1番目、2番目に定義されているものを使う。
        /// 「F.更新後データ」にテーブル定義がない場合は ValidateStorage の実行は行われない。
        /// </summary>
        /// <typeparam name="T1">actionに渡す第一引数の型</typeparam>
        /// <typeparam name="T2">actionに渡す第二引数の型</typeparam>
        /// <param name="action">テスト対象処理</param>
        /// <returns>FixtureBookのインスタンス</returns>
        public static FixtureBook Expect<T1, T2>(Action<T1, T2> action)
        {
            FixtureBook fixtureBook = new FixtureBook(true);
            fixtureBook.testCase.Expect(action, typeof(T1), typeof(T2));
            return fixtureBook;
        }

        /// <summary>
        /// GetObject / GetList / GetArray で取得したオブジェクトを引数にして action を実行し、
        /// ValidateStorage の実行を行う。
        /// GetObject / GetList / GetArray で取得するオブジェクトは
        /// それぞれ「D.パラメタ」の1番目、2番目、3番目に定義されているものを使う。
        /// 「F.更新後データ」にテーブル定義がない場合は ValidateStorage の実行は行われない。
        /// </summary>
        /// <typeparam name="T1">actionに渡す第一引数の型</typeparam>
        /// <typeparam name="T2">actionに渡す第二引数の型</typeparam>
        /// <typeparam name="T3">actionに渡す第三引数の型</typeparam>
        /// <param name="action">テスト対象処理</param>
        /// <returns>FixtureBookのインスタンス</returns>
        public static FixtureBook Expect<T1, T2, T3>(Action<T1, T2, T3> action)
        {
            FixtureBook fixtureBook = new FixtureBook(true);
            fixtureBook.testCase.Expect(action, typeof(T1), typeof(T2), typeof(T3));
            return fixtureBook;
        }

        /// <summary>
        /// GetObject / GetList / GetArray で取得したオブジェクトを引数にして action を実行し、
        /// ValidateStorage の実行を行う。
        /// GetObject / GetList / GetArray で取得するオブジェクトは
        /// それぞれ「D.パラメタ」の1番目、2番目、3番目、4番目に定義されているものを使う。
        /// 「F.更新後データ」にテーブル定義がない場合は ValidateStorage の実行は行われない。
        /// </summary>
        /// <typeparam name="T1">actionに渡す第一引数の型</typeparam>
        /// <typeparam name="T2">actionに渡す第二引数の型</typeparam>
        /// <typeparam name="T3">actionに渡す第三引数の型</typeparam>
        /// <typeparam name="T4">actionに渡す第四引数の型</typeparam>
        /// <param name="action">テスト対象処理</param>
        /// <returns>FixtureBookのインスタンス</returns>
        public static FixtureBook Expect<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action)
        {
            FixtureBook fixtureBook = new FixtureBook(true);
            fixtureBook.testCase.Expect(action, typeof(T1), typeof(T2), typeof(T3), typeof(T4));
            return fixtureBook;
        }

        /// <summary>
        /// 「B.テストデータクリア条件」と「C.テストデータ」でデータベーステーブルの登録を行い、
        /// 「D.パラメタ」に定義されたオブジェクトを引数にしてテスト対象メソッドを実行し、
        /// テスト対象メソッドの戻り値を「E.取得データ」の値で検証し、
        /// 「F.更新後データ」にテーブル定義があれば データベースの値検証を行う。
        /// 
        /// テスト対象メソッドは、テストクラス名およびテストカテゴリ（シート）名からテスト対象クラスおよびメソッドを類推して実行する。
        /// 例えば、テストクラス名が 「ExampleTest」で、テストカテゴリ（シート）名が「Execute」の場合、
        /// Example クラスの Execute メソッドが実行される。
        /// </summary>
        /// <returns>FixtureBookのインスタンス</returns>
        public static FixtureBook ExpectReturn()
        {
            return ExpectReturn(null, null);
        }

        /// <summary>
        /// 「B.テストデータクリア条件」と「C.テストデータ」でデータベーステーブルの登録を行い、
        /// 「D.パラメタ」に定義されたオブジェクトを引数にしてテスト対象メソッドを実行し、
        /// テスト対象メソッドの戻り値を「E.取得データ」の値で検証し、
        /// 「F.更新後データ」にテーブル定義があれば データベースの値検証を行う。
        /// </summary>
        /// <param name="targetClass">テスト対象クラス</param>
        /// <param name="targetMethod">テスト対象メソッド</param>
        /// <param name="targetMethodParameter">テスト対象メソッドのパラメタ</param>
        /// <returns>FixtureBookのインスタンス</returns>
        public static FixtureBook ExpectReturn(Type targetClass, string targetMethod, params Type[] targetMethodParameter)
        {
            FixtureBook fixtureBook = new FixtureBook(true);
            if (targetClass == null)
            {
                targetClass = fixtureBook.GetDefaultTargetClass();
            }
            if (targetMethod == null)
            {
                targetMethod = fixtureBook.GetDefaultTargetMethod(targetClass);
            }
            fixtureBook.testCase.ExpectReturn(targetClass, targetMethod, targetMethodParameter);
            return fixtureBook;
        }

        /// <summary>
        /// Setup を行い、func を実行し、Validate で戻り値を検証し、ValidateStorage を行う。
        /// ただし、「F.更新後データ」にテーブル定義がない場合は ValidateStorage の実行は行われない。
        /// </summary>
        /// <typeparam name="TResult">funcの戻り値の型</typeparam>
        /// <param name="func">テスト対象処理</param>
        /// <returns>FixtureBookのインスタンス</returns>
        public static FixtureBook ExpectReturn<TResult>(Func<TResult> func)
        {
            FixtureBook fixtureBook = new FixtureBook(true);
            fixtureBook.testCase.ExpectReturn(func);
            return fixtureBook;
        }

        /// <summary>
        /// GetObject / GetList / GetArray で取得したオブジェクトを引数にして func を実行し、
        /// Validate で戻り値を検証し、ValidateStorage の実行を行う。
        /// GetObject / GetList / GetArray で取得するオブジェクトは
        /// 「D.パラメタ」の最初に定義されているものを使う。
        /// 「F.更新後データ」にテーブル定義がない場合は ValidateStorage の実行は行われない。
        /// </summary>
        /// <typeparam name="T">funcに渡す引数の型</typeparam>
        /// <typeparam name="TResult">funcの戻り値の型</typeparam>
        /// <param name="func">テスト対象処理</param>
        /// <returns>FixtureBookのインスタンス</returns>
        public static FixtureBook ExpectReturn<T, TResult>(Func<T, TResult> func)
        {
            FixtureBook fixtureBook = new FixtureBook(true);
            fixtureBook.testCase.ExpectReturn(func, typeof(T));
            return fixtureBook;
        }

        /// <summary>
        /// GetObject / GetList / GetArray で取得したオブジェクトを引数にして func を実行し、
        /// Validate で戻り値を検証し、ValidateStorage の実行を行う。
        /// GetObject / GetList / GetArray で取得するオブジェクトは
        /// それぞれ「D.パラメタ」の1番目、2番目に定義されているものを使う。
        /// 「F.更新後データ」にテーブル定義がない場合は ValidateStorage の実行は行われない。
        /// </summary>
        /// <typeparam name="T1">funcに渡す第一引数の型</typeparam>
        /// <typeparam name="T2">funcに渡す第二引数の型</typeparam>
        /// <typeparam name="TResult">funcの戻り値の型</typeparam>
        /// <param name="func">テスト対象処理</param>
        /// <returns>FixtureBookのインスタンス</returns>
        public static FixtureBook ExpectReturn<T1, T2, TResult>(Func<T1, T2, TResult> func)
        {
            FixtureBook fixtureBook = new FixtureBook(true);
            fixtureBook.testCase.ExpectReturn(func, typeof(T1), typeof(T2));
            return fixtureBook;
        }

        /// <summary>
        /// GetObject / GetList / GetArray で取得したオブジェクトを引数にして func を実行し、
        /// Validate で戻り値を検証し、ValidateStorage の実行を行う。
        /// GetObject / GetList / GetArray で取得するオブジェクトは
        /// それぞれ「D.パラメタ」の1番目、2番目、3番目に定義されているものを使う。
        /// 「F.更新後データ」にテーブル定義がない場合は ValidateStorage の実行は行われない。
        /// </summary>
        /// <typeparam name="T1">funcに渡す第一引数の型</typeparam>
        /// <typeparam name="T2">funcに渡す第二引数の型</typeparam>
        /// <typeparam name="T3">funcに渡す第三引数の型</typeparam>
        /// <typeparam name="TResult">funcの戻り値の型</typeparam>
        /// <param name="func">テスト対象処理</param>
        /// <returns>FixtureBookのインスタンス</returns>
        public static FixtureBook ExpectReturn<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func)
        {
            FixtureBook fixtureBook = new FixtureBook(true);
            fixtureBook.testCase.ExpectReturn(func, typeof(T1), typeof(T2), typeof(T3));
            return fixtureBook;
        }

        /// <summary>
        /// GetObject / GetList / GetArray で取得したオブジェクトを引数にして func を実行し、
        /// Validate で戻り値を検証し、ValidateStorage の実行を行う。
        /// GetObject / GetList / GetArray で取得するオブジェクトは
        /// それぞれ「D.パラメタ」の1番目、2番目、3番目、4番目に定義されているものを使う。
        /// 「F.更新後データ」にテーブル定義がない場合は ValidateStorage の実行は行われない。
        /// </summary>
        /// <typeparam name="T1">funcに渡す第一引数の型</typeparam>
        /// <typeparam name="T2">funcに渡す第二引数の型</typeparam>
        /// <typeparam name="T3">funcに渡す第三引数の型</typeparam>
        /// <typeparam name="T4">funcに渡す第四引数の型</typeparam>
        /// <typeparam name="TResult">funcの戻り値の型</typeparam>
        /// <param name="func">テスト対象処理</param>
        /// <returns>FixtureBookのインスタンス</returns>
        public static FixtureBook ExpectReturn<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> func)
        {
            FixtureBook fixtureBook = new FixtureBook(true);
            fixtureBook.testCase.ExpectReturn(func, typeof(T1), typeof(T2), typeof(T3), typeof(T4));
            return fixtureBook;
        }

        /// <summary>
        /// 「B.テストデータクリア条件」と「C.テストデータ」でデータベーステーブルの登録を行い、
        /// 「D.パラメタ」に定義されたオブジェクトを引数にして、指定されたテスト対象メソッドを実行し、
        /// テスト対象メソッドで発生した例外を「E.取得データ」の値で検証し、
        /// 「F.更新後データ」にテーブル定義があれば データベースの値検証を行う。
        /// 
        /// テスト対象メソッドは、テストクラス名およびテストカテゴリ（シート）名からテスト対象クラスおよびメソッドを類推して実行する。
        /// 例えば、テストクラス名が 「ExampleTest」で、テストカテゴリ（シート）名が「Execute」の場合、
        /// Example クラスの Execute メソッドが実行される。
        /// </summary>
        /// <returns>FixtureBookのインスタンス</returns>
        public static FixtureBook ExpectThrown()
        {
            return ExpectThrown<Exception>(null, null);
        }

        /// <summary>
        /// 「B.テストデータクリア条件」と「C.テストデータ」でデータベーステーブルの登録を行い、
        /// 「D.パラメタ」に定義されたオブジェクトを引数にして、指定されたテスト対象メソッドを実行し、
        /// テスト対象メソッドで発生した例外を「E.取得データ」の値で検証し、
        /// 「F.更新後データ」にテーブル定義があれば データベースの値検証を行う。
        /// </summary>
        /// <typeparam name="TException">発生が予想される例外</typeparam>
        /// <param name="targetClass">テスト対象クラス</param>
        /// <param name="targetMethod">テスト対象メソッド</param>
        /// <param name="targetMethodParameter">テスト対象メソッドのパラメタ</param>
        /// <returns>FixtureBook のインスタンス</returns>
        public static FixtureBook ExpectThrown<TException>(Type targetClass, string targetMethod, params Type[] targetMethodParameter) where TException : Exception
        {
            FixtureBook fixtureBook = new FixtureBook(true);
            if (targetClass == null)
            {
                targetClass = fixtureBook.GetDefaultTargetClass();
            }
            if (targetMethod == null)
            {
                targetMethod = fixtureBook.GetDefaultTargetMethod(targetClass);
            }
            fixtureBook.testCase.ExpectThrown<TException>(targetClass, targetMethod, targetMethodParameter);
            return fixtureBook;	
        }

        /// <summary>
        /// action を実行し、
        /// Validate&lt;TException&gt; で想定される例外を検証し、ValidateStorage の実行を行う。
        /// 「F.更新後データ」にテーブル定義がない場合は ValidateStorage の実行は行われない。
        /// </summary>
        /// <typeparam name="TException">発生が予想される例外</typeparam>
        /// <param name="action">テスト対象処理</param>
        /// <returns>FixtureBookのインスタンス</returns>
        public static FixtureBook ExpectThrown<TException>(Action action) where TException : Exception
        {
            FixtureBook fixtureBook = new FixtureBook(true);
            fixtureBook.testCase.ExpectThrown<TException>(action);
            return fixtureBook;
        }

        /// <summary>
        /// GetObject / GetList / GetArray で取得したオブジェクトを引数にして action を実行し、
        /// Validate&lt;TException&gt; で想定される例外を検証し、ValidateStorage の実行を行う。
        /// GetObject / GetList / GetArray で取得するオブジェクトは
        /// それぞれ「D.パラメタ」の最初に定義されているものを使う。
        /// 「F.更新後データ」にテーブル定義がない場合は ValidateStorage の実行は行われない。
        /// </summary>
        /// <typeparam name="T">actionに渡す引数の型</typeparam>
        /// <typeparam name="TException">発生が予想される例外</typeparam>
        /// <param name="action">テスト対象処理</param>
        /// <returns>FixtureBookのインスタンス</returns>
        public static FixtureBook ExpectThrown<T, TException>(Action<T> action) where TException : Exception
        {
            FixtureBook fixtureBook = new FixtureBook(true);
            fixtureBook.testCase.ExpectThrown<TException>(action, typeof(T));
            return fixtureBook;
        }

        /// <summary>
        /// GetObject / GetList / GetArray で取得したオブジェクトを引数にして action を実行し、
        /// Validate&lt;TException&gt; で想定される例外を検証し、ValidateStorage の実行を行う。
        /// GetObject / GetList / GetArray で取得するオブジェクトは
        /// それぞれ「D.パラメタ」の1番目、2番目に定義されているものを使う。
        /// 「F.更新後データ」にテーブル定義がない場合は ValidateStorage の実行は行われない。
        /// </summary>
        /// <typeparam name="T1">actionに渡す第一引数の型</typeparam>
        /// <typeparam name="T2">actionに渡す第二引数の型</typeparam>
        /// <typeparam name="TException">発生が予想される例外</typeparam>
        /// <param name="action">テスト対象処理</param>
        /// <returns>FixtureBookのインスタンス</returns>
        public static FixtureBook ExpectThrown<T1, T2, TException>(Action<T1, T2> action) where TException : Exception
        {
            FixtureBook fixtureBook = new FixtureBook(true);
            fixtureBook.testCase.ExpectThrown<TException>(action, typeof(T1), typeof(T2));
            return fixtureBook;
        }

        /// <summary>
        /// GetObject / GetList / GetArray で取得したオブジェクトを引数にして action を実行し、
        /// Validate&lt;TException&gt; で想定される例外を検証し、ValidateStorage の実行を行う。
        /// GetObject / GetList / GetArray で取得するオブジェクトは
        /// それぞれ「D.パラメタ」の1番目、2番目、3番目に定義されているものを使う。
        /// 「F.更新後データ」にテーブル定義がない場合は ValidateStorage の実行は行われない。
        /// </summary>
        /// <typeparam name="T1">actionに渡す第一引数の型</typeparam>
        /// <typeparam name="T2">actionに渡す第二引数の型</typeparam>
        /// <typeparam name="T3">actionに渡す第三引数の型</typeparam>
        /// <typeparam name="TException">発生が予想される例外</typeparam>
        /// <param name="action">テスト対象処理</param>
        /// <returns>FixtureBookのインスタンス</returns>
        public static FixtureBook ExpectThrown<T1, T2, T3, TException>(Action<T1, T2, T3> action) where TException : Exception
        {
            FixtureBook fixtureBook = new FixtureBook(true);
            fixtureBook.testCase.ExpectThrown<TException>(action, typeof(T1), typeof(T2), typeof(T3));
            return fixtureBook;
        }

        /// <summary>
        /// GetObject / GetList / GetArray で取得したオブジェクトを引数にして action を実行し、
        /// Validate&lt;TException&gt; で想定される例外を検証し、ValidateStorage の実行を行う。
        /// GetObject / GetList / GetArray で取得するオブジェクトは
        /// それぞれ「D.パラメタ」の1番目、2番目、3番目、4番目に定義されているものを使う。
        /// 「F.更新後データ」にテーブル定義がない場合は ValidateStorage の実行は行われない。
        /// </summary>
        /// <typeparam name="T1">actionに渡す第一引数の型</typeparam>
        /// <typeparam name="T2">actionに渡す第二引数の型</typeparam>
        /// <typeparam name="T3">actionに渡す第三引数の型</typeparam>
        /// <typeparam name="T4">actionに渡す第四引数の型</typeparam>
        /// <typeparam name="TException">発生が予想される例外</typeparam>
        /// <param name="action">テスト対象処理</param>
        /// <returns>FixtureBookのインスタンス</returns>
        public static FixtureBook ExpectThrown<T1, T2, T3, T4, TException>(Action<T1, T2, T3, T4> action) where TException : Exception
        {
            FixtureBook fixtureBook = new FixtureBook(true);
            fixtureBook.testCase.ExpectThrown<TException>(action, typeof(T1), typeof(T2), typeof(T3), typeof(T4));
            return fixtureBook;
        }

        /// <summary>
        /// Expect, ExpectReturn, ExpectThrown を実行した際の引数を取得する。
        /// </summary>
        /// <example>
        /// 利用例：
        /// <code>
        /// Data data = FixtureBook.Expect(...).GetParameterAt&lt;Data&gt;(0); // 第1引数を取得する
        /// </code>
        /// </example>
        /// <typeparam name="T">取得する引数の型</typeparam>
        /// <param name="index">取得する引数のインデックス</param>
        /// <returns>引数の値</returns>
        public T GetParameterAt<T>(int index)
        {
            InitializeIfNotYet();
            return testCase.GetParameterAt<T>(index);
        }

        /// <summary>
        /// Expect, ExpectReturn, ExpectThrown を実行した後の引数の値が
        /// 「E.取得データ」に記述された値と同じになっているかどうかを検証する。
        /// このメソッドでは、「E.取得データ」のテーブル定義名は「D.パラメタ」の定義名と同じものとみなされる。
        /// </summary>
        /// <example>
        /// 利用例：
        /// <code>
        /// FixtureBook.Expect(...).ValidateParameterAt(0, 1); // 第1引数、第2引数を検証する
        /// </code>
        /// </example>
        /// <param name="index">検証する引数のインデックス</param>
        public FixtureBook ValidateParameterAt(params int[] index)
        {
            InitializeIfNotYet();
            foreach (int i in index)
            {
                testCase.ValidateParameterAt(i);
            }
            return this;
        }

        /// <summary>
        /// Expect, ExpectReturn, ExpectThrown を実行した後の引数の値が
        /// 「E.取得データ」に記述された値と同じになっているかどうかを検証する。
        /// </summary>
        /// <example>
        /// 利用例：
        /// <code>
        /// FixtureBook.Expect(...).ValidateParameterAt(0, "Data1"); // 第1引数を "Data1" という名前のテーブル定義で検証する
        /// </code>
        /// </example>
        /// <param name="index">検証する引数のインデックス</param>
        /// <param name="name">テーブル定義名</param>
        /// <returns>このインスタンス</returns>
        public FixtureBook ValidateParameterAt(int index, string name)
        {
            InitializeIfNotYet();
            testCase.ValidateParameterAt(index, name);
            return this;
        }
    }
}
