
### チュートリアル#03

DataSet / DataTableを使った開発での利用例
=========================================

ここでは、DataSet / DataTable を使って開発を行っているプロジェクトでの利用例を説明します。


テスト対象のアプリケーション
----------------------------

このチュートリアルでは、以下のような構成のアプリケーションを対象にテストを行います。


### テーブル

    CREATE TABLE EMPLOYEE (
        ID              NUMBER(8)       PRIMARY KEY,
        NAME            VARCHAR2(40)    NOT NULL,
        AGE             NUMBER(3)       NOT NULL,
        RETIRE          NUMBER(1)       NOT NULL,
        LAST_UPDATED    TIMESTAMP       NOT NULL
    );

データベース環境としては Oracle を使用します。


### データベース操作を行うクラス（テスト対象のクラス）

```c#
    using Oracle.DataAccess.Client;
    using System;
    using System.Configuration;
    using System.Data;
    
    namespace TutorialDataSet
    {
        public class EmployeeStore
        {
            public void Save(DataSet dataSet)
            {
                (...)
            }
            
            public void Delete(DataTable parameter)
            {
                (...)
            }
            
            public DataTable GetAllEmployees()
            {
                (...)
            }
            
            public DataSet GetEmployees(DataTable parameter)
            {
                (...)
            }
            
            private OracleConnection GetConnection()
            {
                OracleConnection connection = new OracleConnection();
                connection.ConnectionString = ConfigurationManager.ConnectionStrings[0].ConnectionString;
                connection.Open();
                return connection;
            }
        }
    }
```

このクラスでは以下の処理を行います。

*   Save は引数に指定された従業員データの保存を行う。
*   Delete  は引数に指定された従業員データの削除を行う。
*   GetAllEmployees はデータベーステーブル上に登録されている従業員データを全て取得する。
*   GetEmployees では、引数に指定されている従業員情報の RETIRE 列の値を条件にして従業員データを抽出する。

なお、引数や戻り値の型が DataTable だったり、DataSet だったりして一貫性がないのは、
DataTable と DataSet 両方の利用例を説明する目的のためであり、それ以外の意図はありません。



FixtureBook の作成
------------------

[単体テストプロジェクトの準備](./Tutorial-Setup.md)ができたら、
まず FixtureBook (テストデータを記述する Excel ファイル) を作成します。

ファイルは `.xlsx` 形式で作成してください。
ファイルの作成場所は単体テストクラスのあるフォルダ、ファイル名は「単体テストクラス名.xlsx」とすると、
単体テストクラスでのファイルパス指定が省略できるため、おすすめです。
例えば、単体テストクラスを `EmployeeStorTest.cs` というファイルに作成する場合、
FixtureBook のファイル名は `EmployeeStoreTest.xlsx` として同じフォルダに格納してください。



単体テストクラスの作成
----------------------

次に単体テストクラスを作成します。
作成した単体テストクラスで FixtureBook を利用可能にするために、
以下のような修正を加えます。

1.  `using XPFriend.Fixture;` を追加する。
2.  `private FixtureBook fixtureBook = new FixtureBook();` といった形で fixtureBook フィールドを追加する。

```c#
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using XPFriend.Fixture;

    namespace TutorialDataSetTest
    {
        [TestClass]
        public class EmployeeStoreTest
        {
            private FixtureBook fixtureBook = new FixtureBook();
        }
    }
```


実際のテストの例
----------------

これで FixtureBook の利用準備ができました。
実際のテスト方法については以下のドキュメントに記載されていますので、そちらでどうぞ。

*   [#03-01 データ登録メソッド Save のテスト](./Tutorial-DataSet-Save.md)
*   [#03-02 データ削除メソッド Delete のテスト](./Tutorial-DataSet-Delete.md)
*   [#03-03 データ取得メソッド GetAllEmployees のテスト](./Tutorial-DataSet-GetAllEmployees.md)
*   [#03-04 データ検索メソッド GetEmployees のテスト](./Tutorial-DataSet-GetEmployees.md)
*   [#03-05 例外発生のテスト](./Tutorial-DataSet-Exception.md)
*   [#03-06 テストメソッドの簡略化](./Tutorial-DataSet-Expect.md)

