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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using XPFriend.Fixture;
using XPFriend.FixtureTest.Tutorial;
using XPFriend.Junk;

namespace XPFriend.FixtureTest
{
    public class Wrapper
    {
        public void Save(List<Employee> list)
        {
            new TestTargetClassExample().Save(list);
            TestTargetClassExampleVATest.save1 = true;
        }

        public void Save(List<Employee> list, string s)
        {
            Assert.AreEqual("a", s);
            new TestTargetClassExample().Save(list);
            TestTargetClassExampleVATest.save2 = true;
        }

        public List<Employee> GetEmployees(Employee employee)
        {
            try
            {
                List<Employee> employees = new List<Employee>();
                employees.Add(employee);
                return new TestTargetClassExample().GetEmployees(employees);
            }
            finally
            {
                TestTargetClassExampleVATest.getEmployees = true;
            }
        }

        public void Delete(List<Employee> employees)
        {
            try
            {
                new TestTargetClassExample().Delete(employees);
            }
            finally
            {
                TestTargetClassExampleVATest.delete1 = true;
            }
        }

        public void Delete(List<Employee> employees, int i)
        {
            Assert.AreEqual(1, i);
            TestTargetClassExampleVATest.delete2 = true;
        }
    }

    public class Uncreatable
    {
        public Uncreatable(string s) { }
        public void Save(List<Employee> list) { }
    }

    [TestClass]
    public class TestTargetClassExampleVATest
    {
	    public static bool save1;
        public static bool save2;
        public static bool getEmployees;
        public static bool delete1;
        public static bool delete2;

        [TestMethod]
        public void Expect__テスト対象クラスとテストメソッドを引数指定したExpectは指定されたメソッドを実行する_引数1個の場合()
        {
            // setup
            save1 = false;
            // when
            FixtureBook.Expect(typeof(Wrapper), "Save", typeof(List<Employee>));
            // then
            Assert.IsTrue(save1);
        }

        [TestMethod]
        [Fixture("Expect", "テスト対象クラスとテストメソッドを引数指定したExpectは指定されたメソッドを実行する_引数1個の場合")]
        public void Expect__指定されたクラスのインスタンスが作成できない場合は例外をスローする()
        {
            try
            {
                // when
                FixtureBook.Expect(typeof(Uncreatable), "Save", typeof(List<Employee>));
                Assert.Fail("ここにはこない");
            }
            catch(ConfigException e)
            {
                // then
                Console.WriteLine(e);
                Assert.AreEqual("M_Fixture_Temp_Conductor_CannotCreateInstance", e.ResourceKey);
            }
        }

        [TestMethod]
        [Fixture("Expect", "テスト対象クラスとテストメソッドを引数指定したExpectは指定されたメソッドを実行する_引数1個の場合")]
        public void Expect__指定されたメソッドがみつからない場合は例外をスローする_メソッド引数指定ありの場合()
        {
            try
            {
                // when
                FixtureBook.Expect(typeof(Wrapper), "Save", typeof(String), typeof(String));
            }
            catch (ConfigException e)
            {
                // then
                Console.WriteLine(e);
                Assert.AreEqual("M_Fixture_Temp_Conductor_CannotFindMethod", e.ResourceKey);
            }
    	}

        [TestMethod]
        [Fixture("Expect", "テスト対象クラスとテストメソッドを引数指定したExpectは指定されたメソッドを実行する_引数1個の場合")]
        public void Expect__指定されたメソッドがみつからない場合は例外をスローする_メソッド引数指定なしの場合()
        {
            try
            {
                // when
                FixtureBook.Expect(typeof(Wrapper), "ｘｘｘｘ");
            }
            catch (ConfigException e)
            {
                // then
                Console.WriteLine(e);
                Assert.AreEqual("M_Fixture_Temp_Conductor_CannotFindMethod", e.ResourceKey);
            }
        }

        [TestMethod]
        public void Expect__テスト対象クラスとテストメソッドを引数指定したExpectは指定されたメソッドを実行する_引数2個の場合()
        {
            // setup
            save2 = false;
            // when
            FixtureBook.Expect(typeof(Wrapper), "Save", typeof(List<Employee>), typeof(String));
            // then
            Assert.IsTrue(save2);
        }

        [TestMethod]
        public void ExpectReturn__テスト対象クラスとテストメソッドを引数指定したExpectReturnは指定されたメソッドを実行する()
        {
            // setup
            getEmployees = false;
            // when
            FixtureBook.ExpectReturn(typeof(Wrapper), "GetEmployees", typeof(Employee));
            // then
            Assert.IsTrue(getEmployees);
        }

        [TestMethod]
        public void ExpectThrown__テスト対象クラスとテストメソッドを引数指定したExpectThrownは指定されたメソッドを実行する_正常()
        {
            // setup
            delete1 = false;
            // when
            FixtureBook.ExpectThrown<ApplicationException>(typeof(Wrapper), "Delete", typeof(List<Employee>));
            // then
            Assert.IsTrue(delete1);
        }

        [TestMethod]
        public void ExpectThrown__テスト対象クラスとテストメソッドを引数指定したExpectThrownは指定されたメソッドを実行する_エラー()
        {
            // setup
            delete2 = false;
            try
            {
                // when
                FixtureBook.ExpectThrown<ApplicationException>(typeof(Wrapper), "Delete", typeof(List<Employee>), typeof(int));
                throw new Exception("ここにはこない");
            }
            catch (AssertFailedException e)
            {
                // then
                Console.WriteLine(e);
                Assert.IsTrue(delete2);
            }
        }
    }
}
