
### チュートリアル#02-01

データ登録メソッド Save のテスト
================================

テスト対象のメソッド
--------------------

ここでは、以下の Save メソッドを対象にテストを行います。

```c#
    namespace TutorialCodeFirst
    {
        public class EmployeeStore
        {
            public void Save(List<Employee> employees)
            {
                using (MyAppDbContext context = new MyAppDbContext())
                {
                    employees.ForEach(employee => employee.LastUpdated = DateTime.Now);
                    context.Employees.AddOrUpdate(employees.ToArray());
                    context.SaveChanges();
                }
            }

            (...)
        }
    }
```

Save メソッドは、引数として渡された従業員データ (Employee) をデータベーステーブル (Employees) に登録します。
この際、引数の従業員データのIdと同じものがデータベーステーブル上にあれば更新を行い、なければ追加を行います。


テストケース
------------

まずは以下のケースについてテストしたいと思います。

*   <b>データベーステーブル (Employees) に存在しないIdの従業員データを渡した場合には新規追加される</b>


テストプログラムの処理
----------------------

上記のテストケースをテストするために、テストプログラムでは以下のことを行う必要があります。

1.  事前にデータベーステーブル (Employees) 内のデータを削除しておく
    (「Saveメソッドの実行によりデータが登録された」ということを確認するため）。
2.  Saveメソッドの引数として渡す Employee のリストを作成する。
3.  Saveメソッドを呼び出す。
4.  Saveメソッドの呼び出しの結果、データベーステーブル (Employees) にデータ登録されたことを確認する。

上記の処理の 3 以外はいずれもテストデータを扱う処理となります。
FixtureBook には、これらのデータを記述します。



FixtureBook の記述
------------------

FixtureBook の記述内容は以下の通りです。

*   データベーステーブル (Employees) の行削除条件を `B.テストデータクリア条件` に記述する。
*   Save メソッドの引数として渡すデータを `D.パラメタ` に記述する。
*   Save メソッドの実行後の Employees テーブルのデータ状態を予想して`F.更新後データ`に記述する。

![FixtureBook記述](./images/Tutorial-CodeFirst-Save-01.png?raw=true)


### B.テストデータクリア条件

ここでは、Employees テーブルの行データをどういう条件で削除するかを指定します。

![B.テストデータクリア条件](./images/Tutorial-CodeFirst-Save-02.png?raw=true)

この例では、主キー項目である Id 列に `*` を指定することですべての行を削除するように指定しています。


### D.パラメタ

Save メソッドの引数は、Employee クラスのリスト (List<Employee>) ですが、
このデータの内容は `D.パラメタ` に記述します。

![D.パラメタ](./images/Tutorial-CodeFirst-Save-03.png?raw=true)

Employee クラスには、以下のように Id, Name, Age, Retire, Lastupdated の5つのプロパティがありまが、
Id はデータベースにより自動生成され、LastUpdated は Save メソッドの中で現在日時が設定されるため、
上記の定義では Name, Age, Retire の 3項目のみについて記述しています。

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

なお、`B.テストデータクリア条件`および`F.更新後データ`が "Employees" になっている部分が
`D.パラメタ` では "Employee" になっているのは、間違いではありません。
`B.テストデータクリア条件`および `F.更新後データ` の "Employees" はデータベーステーブル名を表しているのに対して、
`D.パラメタ`は Employee クラスの名前を表しているためです。


### F.更新後データ

`F.更新後データ`には、Save メソッド実行後の予想結果 (Employees テーブルのあるべき状態) を記述します。

![F.更新後データ](./images/Tutorial-CodeFirst-Save-04.png?raw=true)

説明の先頭に \* が付いている項目 (この例では `*Name` ) は検索条件指定列を意味します。
FixtureBook がデータベーステーブル上のデータが正しいかどうかのチェックを行う際には、
まず、この列に指定された値を条件にしてデータベースを検索し、
次に、取得したデータ行の各列値について、Excel セル上の値と等しいかどうかを比較チェックします。

Id列の値 (予想結果) として指定されている `*` は「何らかの値があればOK」を意味します。
Id列の値はデータベースにより自動生成される設定になっているため、値が何になるかがわからないためです。

LastUpdated列の値として指定されている `${TODAY}` は「本日の日時であればOK」を意味します。
LastUpdated列に関してはSaveメソッドにより現在日時が自動設定されるため、正確な時間を予想することが難しいからです。


テストメソッドの記述
--------------------

次に単体テストクラスにテストメソッドを追加します。

テストメソッドの名前は、以下のように `[シート名]__[テストケース記述]` の形式で指定してください。

    [TestMethod]
    public void Save__データベーステーブルEmployeesに存在しないIdの従業員データを渡した場合には新規追加される()

テストケース記述にメソッド名として利用できない文字 (空白や記号等) が含まれている場合は、
以下のように `[Fixture]` 属性を使用してテストケースを指定してください。

    [TestMethod]
    [Fixture("Save", "データベーステーブル (Employees) に存在しない Id の従業員データを渡した場合、新規追加される")]
    public void Save__データベーステーブルEmployeesに存在しないIdの従業員データを渡した場合には新規追加される()

テストメソッドは以下の内容で記述します。

1.  Save メソッドの引数として渡すデータを `fixtureBook.GetList<Employee>()` で取得する。
    これにより、`D.パラメタ` で記述した値で初期化された `List<Employee>` が取得できる。
2.  上記で取得した `List<Employee>` を引数にして、テスト対象メソッド (Save) を呼び出す。
3.  `fixtureBook.ValidateStorage()` の呼び出しにより、データベーステーブルのデータ状態を検証する。
    これにより、`F.更新後データ`に記述した内容と、
    実際のデータベーステーブルの内のデータが同じかどうかのチェックが行われる。

```
    [TestMethod]
    public void Save__データベーステーブルEmployeesに存在しないIdの従業員データを渡した場合には新規追加される()
    {
        // setup
        List<Employee> employees = fixtureBook.GetList<Employee>();

        // when
        new EmployeeStore().Save(employees);

        // then
        fixtureBook.ValidateStorage();
    }
```

なお、`B.テストデータクリア条件` に記述された条件でのデータベーステーブルからの行削除は、
上記のテストプログラムでは明示的に行っていませんが、
`fixtureBook.GetList<Employee>()` の呼び出しの中で自動的に実行されます。


テストの実行
------------

テスト実行の方法は、通常の単体テスト実行と同じです。
エラーが出た場合はエラーメッセージを確認して、テスト対象メソッドの内容やテストデータ記述の修正を行ってください。

![エラーメッセージ例](./images/Tutorial-CodeFirst-Save-05.png?raw=true)



テストケースの追加
------------------

次は以下のテストケースを追加してみます。

*   <b>データベーステーブル (Employees) に存在するIdの従業員データを渡した場合、既存データが更新される</b>


FixtureBook への追加
--------------------

FixtureBook のひとつのシートに、複数のテストケースを書くことが可能です。
今回は、先ほどと同じ Save シートに追加のテストケース記述を行いました。

![追加テストケース](./images/Tutorial-CodeFirst-Save-06.png?raw=true)

今回は、テスト前に Employees データベーステーブル上にデータを格納しておく必要があるため、
`C.テストデータ` の記述を追加しています。


テストメソッドの追加
--------------------

テストメソッドの記述内容は、先ほどのテストケースと全く同じです。

```
    [TestMethod]
    public void Save__データベーステーブルEmployeesに存在するIdの従業員データを渡した場合には既存データが更新される()
    {
        // setup
        List<Employee> employees = fixtureBook.GetList<Employee>();

        // when
        new EmployeeStore().Save(employees);

        // then
        fixtureBook.ValidateStorage();
    }
```


`B.テストデータクリア条件` に記述された条件でのデータベーステーブルからの行削除と
`C.テストデータ` に記述されたデータのテーブル追加は、
`fixtureBook.GetList<Employee>()` の呼び出しの中で自動的に実行されます。


リファクタリング
----------------

ここで作成した2つのテストメソッドの記述内容は全く同じため、以下のように共通メソッドにすることが可能です。

```
    [TestMethod]
    public void Save__データベーステーブルEmployeesに存在しないIdの従業員データを渡した場合には新規追加される()
    {
        TestSave();
    }

    [TestMethod]
    public void Save__データベーステーブルEmployeesに存在するIdの従業員データを渡した場合には既存データが更新される()
    {
        TestSave();
    }

    private void TestSave()
    {
        // setup
        List<Employee> employees = fixtureBook.GetList<Employee>();

        // when
        new EmployeeStore().Save(employees);

        // then
        fixtureBook.ValidateStorage();
    }
```

