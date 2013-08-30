
FixtureBook
===========

FixtureBook とは
----------------

FixtureBook とは単体テストで使用するデータを .xlsx ファイルに記述できるようにするための仕組みです。


機能
----

FixtureBook を使うと、.xlsx ファイル上に記述した以下のようなデータを単体テストで簡単に利用することができます。

*   テスト前にDBテーブルに登録しておきたいデータ内容。
*   テスト対象メソッドの引数等として利用するオブジェクトのプロパティ値。
*   テスト対象メソッドを実行して取得できた結果と照合するための予想結果。
*   テスト後のDBテーブルのあるべき状態を表すデータ。


セットアップ方法
----------------

[NuGet](https://www.nuget.org/packages/FixtureBook/) でインストールするのが簡単です。


使い方
------

FixtureBook の使い方はとてもシンプルです。

1.  テストで使用したいデータを FixtureBook (.xlsx ファイル) に記述する。
2.  FixtureBook を利用する単体テストを書く。


FixtureBook 利用例
--------------------

例えば、以下のような「従業員クラス (Employee)」

```c#
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public bool Retire { get; set; }
        public DateTime LastUpdated { get; set; }
    }
```

を利用するメソッド GetEmployees

```c#
    public class EmployeeStore
    {
        /// <summary>
        /// 従業員データの退職フラグを条件にしてデータベースから検索を行う
        /// </summary>
        /// <param name="parameter">退職フラグが設定された従業員データ</param>
        /// <returns>検索条件に合致した従業員データ</returns>
        public List<Employee> GetEmployees(Employee parameter)
        {
            using (MyAppDbContext context = new MyAppDbContext())
            {
                return (from employee in context.Employees
                        where employee.Retire == parameter.Retire
                        orderby employee.Id
                        select employee).ToList();
            }
        }
    }
```

をテストしたい場合の例です。


### FixtureBook 記述例

次のようなシートのある .xlsx ファイルを作ります。

![FixtureBook記述例](./Document/images/README-01.png)

*   `A.テストケース` の<b>C列</b>にテスト内容を一行で記述する。
*   テスト前にデータベース上に設定しておきたいデータを `B.テストデータクリア条件` と `C.テストデータ` に記述する
    （<b>C列</b>にテーブル名、<b>D列</b>以降に列値を指定）。
*   `D.パラメタ`には、メソッドの引数で渡すデータを記述する（<b>C列</b>にクラス名、<b>D列</b>以降にプロパティ値を指定）。
*   `E.取得データ`には、メソッドの戻り値として取得できるデータを予想して記述する（<b>C列</b>にクラス名、<b>D列</b>以降にプロパティ値を指定）。

作成した .xlsx ファイルは単体テストクラスのソースファイルと同じ名前にして（EmployeeStoreTest.cs ならば EmployeeStoreTest.xlsx とする）、
単体テストクラスのソースファイルと同じフォルダに保存します。


### 単体テスト記述例

単体テストは以下のように記述します。

*   `using XPFriend.Fixture;` を追加する。
*   `Excelシート名__テストケース記述` という名前でテストメソッドを作成する。
*   FixtureBook.ExpectReturn メソッドでテスト対象メソッドを呼び出して、予想結果のチェックを行う。

&nbsp;

    ...
    using XPFriend.Fixture;

    ...

    [TestClass]
    public class EmployeeStoreTest
    {
        [TestMethod]
        public void GetEmployees__引数の退職フラグがtrueの場合データベーステーブルEmployees上の退職者のみが取得できる()
        {
            FixtureBook.ExpectReturn((Employee parameter) => new EmployeeStore().GetEmployees(parameter));
        }
    }

このテストを実行すると、以下の処理が行われます。

1.  `B.テストデータクリア条件` に記述された条件でデータベーステーブルからデータ行を削除した後に
    `C.テストデータ` に記述されたデータがデータベーステーブルに追加される。
2.  `D.パラメタ` に記述された内容で Employee クラスのインスタンスが作成され、引数（ `Employee parameter` ）として渡される。
3.  テスト対象処理 `new EmployeeStore().GetEmployees(parameter)` が実行される。
4.  `GetEmployees` の戻り値が `E.取得データ` に記述した内容と合致しているかどうかがチェックされる。

#### 参考
>この例では、`ExpectReturn` を使いましたが、
>戻り値をチェックしない場合には `Expect`、
>例外発生をチェックしたい場合には `ExpectThrown` が利用可能です。


FixtureBook クラスのメソッド
----------------------------

他にも、以下のようなメソッドを使って FixtureBook の操作が可能です。

<table>
  <tr><td>Setup</td><td><code>B.テストデータクリア条件</code>に記述された条件でDBからデータ削除した後に<code>C.テストデータ</code>に記述されたデータをDBに追加する。</td></tr>
  <tr><td>GetObject / GetList / GetArray</td><td><code>D.パラメタ</code>に記述した内容で初期化されたオブジェクトを取得する。</td></tr>
  <tr><td>Validate</td><td>引数に指定されたオブジェクトが<code>E.取得データ</code>に記述した予想結果と同じかどうか検証する。</td></tr>
  <tr><td>ValidateStorage</td><td>DB上のデータが<code>F.更新後データ</code>に記述した予想結果と同じかどうか検証する。</td></tr>
</table>


FixtureBook 属性と Fixture 属性
-------------------------------

以下のように、クラスまたはメソッドに `[FixtureBook]` 属性を指定すると、
利用する .xlsx ファイルのパスが指定できます。

```c#
    [TestClass]
    [FixtureBook("Examples/RetireHandler.xlsx")]
    public class RetireHandlerTest
```

`[FixtureBook]` 属性がクラスとメソッドの両方に付いている場合は、
メソッドで指定されたパスが優先的に利用されます。


「A.テストケース」記述の内容とメソッド名を同じにしたくない（または同じにできない）場合は、
以下のように `[Fixture]` 属性が利用できます。

```c#
    [TestMethod]
    [Fixture("UpdateRetire", "年齢が60才以上ならば退職フラグをtrueにして名前を消去する")]
    public void TestMethod1()
```


利用上の制約
------------

FixtureBook には現在のところ以下の制約があります。

*   Excelファイルは `.xlsx` 形式のみ利用可能 (`.xls` 形式ファイルは利用できない)。
*   利用可能なデータベースは Oracle および SQLServer です。


もっと詳しく!
-------------

FixtureBook について、もっと詳しく知りたいときは、以下のドキュメントも参照してみてください。

### チュートリアル

*   [#01 セットアップ](./Document/Tutorial-Setup.md)
*   [#02 Entity Framework コードファースト開発での利用例](./Document/Tutorial-CodeFirst.md)
*   [#03 DataSet / DataTableを使った開発での利用例](./Document/Tutorial-DataSet.md)


### リファレンス

*   [FixtureBook 記述ルール](./Document/FixtureBookReference.md)
