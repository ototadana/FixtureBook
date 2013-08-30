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
        internal const string NameSeparatorKey = "FixtureBook.nameSeparator";
        internal const string DefaultNameSeparator = "__";
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

        private static string[] GetNamesByAttribute(MemberInfo method)
        {
            FixtureAttribute attribute = (FixtureAttribute)Attribute.GetCustomAttribute(method, typeof(FixtureAttribute));
            if (attribute == null)
            {
                return null;
            }
            return new string[] { attribute.Category, attribute.Description };
        }

        private static string[] GetNamesByTestName(string name)
        {
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
                    return path;
                }
            }

            return GetDefaultFilePath(stackFrame);
        }

        private static string GetDefaultFilePath(StackFrame frame)
        {
            string testClassFileName = frame.GetFileName();
            return Path.Combine(
                Path.GetDirectoryName(testClassFileName),
                Path.GetFileNameWithoutExtension(testClassFileName) + ".xlsx");
        }


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
            String[] name = GetNamesByAttribute(method);
            if(name != null) 
            {
                return CreateFixtureInfo(stackFrame, method, type, name);
            }

            name = GetNamesByTestName(method.Name);
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
            string filePath = PathUtil.EditFilePath(GetFilePath(stackFrame, type, method));
            return new FixtureInfo{FilePath = filePath, SheetName = name[0], TestCaseName = name[1]};
        }

        internal class FixtureInfo 
        {
            public string FilePath {get; set;}
            public string SheetName {get; set;}
            public string TestCaseName {get; set;}
        }

        private void Initialize(FixtureInfo fixtureInfo)
        {
            book = Book.GetInstance(fixtureInfo.FilePath);
            sheet = book.GetSheet(fixtureInfo.SheetName);
            testCase = sheet.GetCase(fixtureInfo.TestCaseName);
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
        /// 「D.パラメタ」に記述されたデータを用いてオブジェクトを生成する。
        /// </summary>
        /// <typeparam name="T">生成するオブジェクトのクラス</typeparam>
        /// <param name="name">テーブル定義名</param>
        /// <returns>生成されたオブジェクト</returns>
        public T GetObject<T>(params string[] name)
        {
            InitializeIfNotYet();
            return testCase.GetObject<T>(name);
        }

        /// <summary>
        /// 「D.パラメタ」に記述されたデータを用いてオブジェクトのリストを生成する。
        /// </summary>
        /// <typeparam name="T">生成するリスト要素オブジェクトクラス</typeparam>
        /// <returns>生成されたリスト</returns>
        public List<T> GetList<T>()
        {
            return GetList<T>(null);
        }

        /// <summary>
        /// 「D.パラメタ」に記述されたデータを用いてオブジェクトのリストを生成する。
        /// </summary>
        /// <typeparam name="T">生成するリスト要素オブジェクトクラス</typeparam>
        /// <param name="name">テーブル定義名</param>
        /// <returns>生成されたリスト</returns>
        public List<T> GetList<T>(string name)
        {
            InitializeIfNotYet();
            return testCase.GetList<T>(name);
        }

        /// <summary>
        /// 「D.パラメタ」に記述されたデータを用いてオブジェクトの配列を生成する。
        /// </summary>
        /// <typeparam name="T">生成する配列要素オブジェクトクラス</typeparam>
        /// <returns>生成された配列</returns>
        public T[] GetArray<T>()
        {
            return GetArray<T>(null);
        }

        /// <summary>
        /// 「D.パラメタ」に記述されたデータを用いてオブジェクトの配列を生成する。
        /// </summary>
        /// <typeparam name="T">生成する配列要素オブジェクトクラス</typeparam>
        /// <param name="name">テーブル定義名</param>
        /// <returns>生成された配列</returns>
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
    }
}
