
FixtureBook 記述ルール
======================

FixtureBook の論理構造
----------------------

FixtureBook は以下のような論理構造になっています。

    - ブック
      - シート
        - テストケース
          - セクション
            - テーブル
              - 列名行
              - データ行


ブック、シート、テストケース
----------------------------

ひとつのテストで使用するデータは<b>テストケース</b>で表します。
ひとつの<b>シート</b>には複数のテストケースを格納することができ、
ひとつの<b>ブック</b>には複数のシートを格納することができます。

これにより、ブック及びシートはそれぞれ<b>大分類</b>、<b>中分類</b>といったテストケースのカテゴリとして利用することができます。

![ブック、シート、テストケース](./images/FixtureBookReference-01.png?raw=true)



### ブック

<b>ブック</b>はテストデータの集合です。
例えば「ひとつの単体テストクラスで扱う全てのデータをひとつのブックに格納する」といった使い方ができます。

ひとつのブックは、ひとつの Excel ファイルに対応します。


### シート

<b>シート</b>はテストデータの集合です。
シートには複数のテストケースを含めることができます。
例えば、「ひとつのメソッドに対するテストケースをひとつのシートに記述する」といった使い方ができます。

ひとつのシートは、ひとつの Excel シートに対応します。


### テストケース

<b>テストケース</b>は、ひとつのテストで使用するテストデータをあらわします。

テストケースは以下の<b>セクション</b>を持ち、その中でテストデータ記述を行います。

<table>
  <tr><td>A.テストケース</td><td>テスト (もしくはテストデータ) の内容を一行の文章として記述します</td></tr>
  <tr><td>B.テストデータクリア条件</td><td>テスト直前に実行される、データベーステーブルの行削除条件をテーブル形式で記述します</td></tr>
  <tr><td>C.テストデータ</td><td>テスト直前に実行される、データベーステーブルへの行追加内容をテーブル形式で記述します</td></tr>
  <tr><td>D.パラメタ</td><td>テスト実行時に (テスト対象メソッドの引数等として) 利用するオブジェクトの内容をテーブル形式で記述します</td></tr>
  <tr><td>E.取得データ</td><td>テスト実行によって (テスト対象メソッドの戻り値等として) 取得したオブジェクトの状態予想 (期待結果) をテーブル形式で記述します</td></tr>
  <tr><td>F.更新後データ</td><td>テスト実行によって変更されたデータベーステーブルの状態予想 (期待結果) をテーブル形式で記述します</td></tr>
</table>



セクション
----------

<b>セクション</b>にはテストに使用するデータの記述を行います。
ひとつのセクションはセクション名の記述部分とセクションデータの記述部分に分かれます。

### セクション名

セクション名は<b>B列</b>に記述します。セクション名として利用可能なものは以下の通りです。

*   A.テストケース
*   B.テストデータクリア条件
*   C.テストデータ
*   D.パラメタ
*   E.取得データ
*   F.更新後データ

![セクション名](./images/FixtureBookReference-02.png?raw=true)

*   実際にセクション名の識別子として利用されているのは、最初の一文字だけです。
    従って、`A.テストケース` の代わりに `A. Test Case` と記述することも可能です。


### セクションデータ

セクションデータは、<b>C列</b>から記述を開始します。
`A.テストケース` には、テストの内容を記述する文章を記述し、それ以外のセクションにはテーブル形式でテストデータを記述します。


#### A.テストケース セクションの記述内容

`A.テストケース` には、テスト (もしくはテストデータ) の内容を一行の簡潔な文章として記述します。
一般的には「～の場合は、～となる」という形式で、テスト条件と予想結果をできるだけ短く記述するのがおすすめです。

![テストケース記述](./images/FixtureBookReference-03.png?raw=true)

テストケースセクションの記述内容は、以下のように単体テストメソッド名の一部として記述します。
これにより、現在のテストメソッドがどのテストケースを対象にしているのかを表します。

    [TestMethod]
    public void UpdateRetire__年齢が60才以上ならば退職フラグをtrueにして名前を消去する()

テストケース記述とメソッド名を同じにしたくない場合は、以下のように `[Fixture]` 属性の引数として記述します。

    [TestMethod]
    [Fixture("UpdateRetire", "年齢が60才以上ならば、退職フラグを true にして名前を消去する")]
    public void TestMethod1()


#### A.テストケース 以外のセクションの記述内容

`A.テストケース` 以外のセクションには、テストデータを<b>テーブル</b>形式で記述します。

ひとつのセクションには複数のテーブルを定義できます。

![テーブル記述](./images/FixtureBookReference-04.png?raw=true)


テーブル
--------

<b>テーブル</b>には、テストで利用するデータを表形式で定義します。

<b>C列</b>にテーブル定義名を記述し、
その下の行の<b>D列</b>以降に列名および列データを定義します。
テーブル定義名を記述した行のすぐ下には列名を記述し、列名行の下にはデータを記述します。
列名行は一行のみ、データ行は複数行記述することが可能です。

![テーブル定義名、列名行、データ行](./images/FixtureBookReference-05.png?raw=true)

データ行の値はD列以降に記述しますが、
削除済みデータを表す `D`、および、データの存在を表す `C` をC列に表すことができます。


### テーブル定義名について

セクションによってテーブル定義名記述内容が異なります。

#### B.テストデータクリア条件、C.テストデータ、F.更新後データ の場合

<b>データベーステーブル名</b>を記述します。

また、[データベーステーブル名]@[接続名] と記述することで、
デフォルトの接続先とは別のデータベースに接続することも可能です。
詳細は[チュートリアル#01 セットアップ](./Tutorial-Setup.md) を参照してください。


#### D.パラメタ、E.取得データ、F.更新後データ の場合

以下のいずれかを指定してください。

1.   任意の名前。<br>
     ここで記述した名前は、
     `fixtureBook.GetObject<ClassA>(テーブル定義名")`、`fixtureBook.GetList<ClassB>("テーブル定義名")`、 
     `fixtureBook.GetArray<ClassB>("テーブル定義名")` 、`fixtureBook.Validate(obj, "テーブル定義名")` 
     の形式でテストプログラムから明示的に指定できます。
2.   GetXXXで指定するクラス名。もしくは Validate の引数で渡したオブジェクトのクラス名。
3.   (DataSet /DataTable を扱う場合のみ) DataTable.TableName (データベーステーブル名) と同じ値。


### B.テストデータクリア条件

`B.テストデータクリア条件` には、テスト直前に実行される、データベーステーブルの行削除条件を記述します。
ここに記述された値はテストデータ削除のための SQL 文 (WHERE句) として利用されます。

![B.テストデータクリア条件](./images/FixtureBookReference-06.png?raw=true)


以下の値が削除条件として指定できます。

<table>
<tr>
  <th>指定できる値</th>
  <th>値の例</th>
  <th>条件タイプ</th>
  <th>生成される SQL 条件</th>
</tr>
<tr>
  <td>任意の値</td>
  <td>abc</td>
  <td>完全一致検索</td>
  <td>where COLUMN1 = 'abc'</td>
</tr>
<tr>
  <td>任意の値 + % (パーセント記号)</td>
  <td>abc%</td>
  <td>前方一致検索</td>
  <td>where COLUMN1 like 'abc%'</td>
</tr>
<tr>
  <td>% + 任意の値</td>
  <td>%abc</td>
  <td>後方一致検索</td>
  <td>where COLUMN1 like '%abc'</td>
</tr>
<tr>
  <td>% + 任意の値 + %</td>
  <td>%abc%</td>
  <td>部分一致検索</td>
  <td>where COLUMN1 like '%abc%'</td>
</tr>
<tr>
  <td>* (アスタリスク)</td>
  <td>*</td>
  <td>NOT NULL 検索</td>
  <td>where COLUMN1 is not null</td>
</tr>
<tr>
  <td>何も書かない</td>
  <td></td>
  <td>NULL 検索</td>
  <td>where COLUMN1 is null</td>
</tr>
</table>



#### 単体テストプログラム内での利用方法

`FixtureBook.Setup()` メソッドの呼び出しにより、
`B.テストデータクリア条件` で記述された条件で SQL (DELETE文) が作成され、
データベーステーブルからの行削除が行われます。

ひとつのテストケースの中に複数のテーブル定義がある場合、
定義されている全てのテーブルに対して行削除処理が行われます。

また、`FixtureBook.GetObject()`、`FixtureBook.GetList()`、 `FixtureBook.GetArray()` 
の初回呼び出し時には自動的に `FixtureBook.Setup()` メソッドの呼び出しが行われるため、
これらのメソッドを呼び出す場合は Setup メソッドの呼び出し記述を省略することができます。



### C.テストデータ

![C.テストデータ](./images/FixtureBookReference-07.png?raw=true)

`C.テストデータ`には、テスト前にデータベーステーブルへ追加しておきたいデータを記述します。
ここで指定した値から追加処理の SQL (INSERT文) が作成されます。


#### 単体テストプログラム内での利用方法

`FixtureBook.Setup()` メソッドの呼び出しにより、
`C.テストデータ` で記述されたデータの登録が行われます。
`B.テストデータクリア条件`の記述がある場合は、データ削除処理を行った後にデータ登録処理が行われます。

ひとつのテストケースの中に複数のテーブル定義がある場合、
定義されている全てのテーブルに対して追加処理が行われます。

また、`FixtureBook.GetObject()`、`FixtureBook.GetList()`、 `FixtureBook.GetArray()` 
の初回呼び出し時には自動的に `FixtureBook.Setup()` メソッドの呼び出しが行われるため、
これらのメソッドを呼び出す場合は Setup メソッドの呼び出し記述を省略することができます。


### D.パラメタ

![D.パラメタ](./images/FixtureBookReference-08.png?raw=true)

`D.パラメタ` には、テスト実行時に (テスト対象メソッドの引数等として) 利用するオブジェクトの内容を記述します。

#### 単体テストプログラム内での利用方法

以下のメソッドで、`D.パラメタ` に書いた内容で初期化されたオブジェクトを取得することができます。

```c#
    // オブジェクトを取得する
    ClassA obj = fixtureBook.GetObject<ClassA>();
    
    // オブジェクトのリストを取得する
    List<ClassA> list = fixtureBook.GetList<ClassA>();
    
    // オブジェクトの配列を取得する
    ClassA[] array = fixtureBook.GetArray<ClassA>();
```

#### 作成可能なオブジェクト

`D.パラメタ` の定義から作成することができるオブジェクトは以下の通りです。

*   POCO オブジェクト
*   Dictionary<string,object> オブジェクト
*   DataTable
*   DataSet
*   string, int 等のオブジェクト

##### POCO オブジェクト

`D.パラメタ` の列名行に記述した名前をプロパティ名、
データ行に記述した値をプロパティ値としてオブジェクトが作成されます。

以下のように GetObject, GetList, GetArray が利用できます。

```c#
    // オブジェクトを取得する
    ClassA obj = fixtureBook.GetObject<ClassA>();
    
    // オブジェクトのリストを取得する
    List<ClassA> list = fixtureBook.GetList<ClassA>();
    
    // オブジェクトの配列を取得する
    ClassA[] array = fixtureBook.GetArray<ClassA>();
```


##### Dictionary<string,object> オブジェクト

`D.パラメタ` の列名行に記述した名前をディクショナリのキー、
データ行に記述した値をディクショナリの値としてオブジェクトが作成されます。

以下のように GetObject, GetList, GetArray が利用できます。

```c#
    // ディクショナリを取得する
    Dictionary<string, object> dictionary = fixtureBook.GetObject<Dictionary<string, object>>();

    // ディクショナリのリストを取得する
    List<Dictionary<string, object>> list = fixtureBook.GetList<Dictionary<string, object>>();

    // ディクショナリの配列を取得する
    Dictionary<string, object>[] array = fixtureBook.GetArray<Dictionary<string, object>>();
```

`D.パラメタ` の列名行での列名を [列名]:[型] の形式 (例えば、`age:int`) で記述することで、
作成するディクショナリ値の型を指定することができます。


##### DataTable

`D.パラメタ` のテーブル定義名を DataTable のテーブル名 (DataTable.TableName)、
列名行に記述した名前を DataTable の列 (DataColumn.ColumnName)、
データ行に記述した値を DataRow の列値としてオブジェクトが作成されます。

GetObject メソッドが利用できます。

```c#
    DataTable dataTable = fixtureBook.GetObject<DataTable>();
```


`D.パラメタ`の列名行での列名を [列名]:[型] の形式 (例えば、`age:int`) で記述することで、
作成する値の型を指定することができます。


##### DataSet

`D.パラメタ`のテーブル定義名を DataTable のテーブル名 (DataTable.TableName)、
列名行に記述した名前を DataTable の列 (DataColumn.ColumnName)、
データ行に記述した値を DataRow の列値としてオブジェクトが作成されます。

GetObject メソッドが利用できます。

```c#
    DataSet dataSet = fixtureBook.GetObject<DataSet>();
```

`D.パラメタ` の列名行での列名を [列名]:[型] の形式 (例えば、`age:int`) で記述することで、
作成する値の型を指定することができます。

同一のテストケースで複数のテーブル定義がある場合に、
GetObject の引数でテーブル定義名を明示的に指定しなければ、
その定義の数分の DataTable をもつ DataSet が作成されます。
以下のように利用するテーブル定義名を明示的に指定した場合は、
その名前をもつテーブル定義のみから作成されます。

```c#
    DataSet dataSet = fixtureBook.GetObject<DataSet>("Table1");
```

##### string, int 等のオブジェクト

`D.パラメタ` の列名行に `-` を指定するとデータ行に記述した値でオブジェクトが作成されます。

![string, int 等のオブジェクト](./images/FixtureBookReference-14.png?raw=true)


以下のように GetObject, GetList, GetArray が利用できます。

```c#
    // stringを取得する
    string obj = fixtureBook.GetObject<String>();

    // stringのリストを取得する
    List<string> list = fixtureBook.GetList<string>();

    // stringの配列を取得する
    string[] array = fixtureBook.GetArray<string>();
```



#### テーブル定義検索ルール

GetXXXメソッドでオブジェクトを作成する際に利用するテーブル定義は以下のルールで決定されます。

1.  `fixtureBook.GetObject<ClassA>("テーブル定義名")` というふうにテーブル定義名を明示的に指定した場合は、
    その名前のテーブル定義を利用する。
2.  `fixtureBook.GetObject<ClassA>()` というふうにテーブル定義名が明示的に指定されなかった場合は、
    以下のルールが適用される。
    1.  同一テストケース、セクション内にテーブル定義がひとつしかなければそのテーブル定義が利用される。
    2.  テーブル定義が複数ある場合、メソッドで指定したクラス名 (この例では ClassA) をテーブル定義名とみなす。


#### 未入力セルの扱い

未入力セルは以下のように扱われます。

*   DataTable, DataSet の場合は DBNull.Value が設定される。
*   DataTable, DataSet 以外の場合は null として扱われる。
*   長さゼロの文字列 ("") を表したい場合は、セルに `${EMPTY}` と記述する。


### E.取得データ

![E.取得データ](./images/FixtureBookReference-09.png?raw=true)

`E.取得データ` には、テスト実行によって (テスト対象メソッドの戻り値等として) 取得したオブジェクトの状態予想 (期待結果) を記述します。


#### 単体テストプログラム内での利用方法

テスト対象メソッドの実行結果として取得したオブジェクトを引数にして `FixtureBook.Validate()` 
メソッドを呼び出せば、`E.取得データ` に記述された予想結果との比較チェックが行われます。

```c#
    // テスト対象メソッドを呼び出す
    ClassA result = sut.Execute();

    // 取得した結果が「E.取得データ」に記述した予想結果と同じかどうか検証する
    fixtureBook.Validate(result);
```


#### 検証可能なオブジェクト

Validate メソッドで検証することができるオブジェクトは以下の通りです。

*    POCO オブジェクト
*    Dictionary<string,object> オブジェクト
*    DataTable
*    DataSet
*    string, int 等のオブジェクト

##### POCO オブジェクト

`E.取得データ` の列名行に記述した名前をプロパティ名、
データ行に記述した予想結果値をプロパティ値として検証を行うことができます。

Validate メソッドの引数には単体オブジェクトの他に、
POCO オブジェクトのリスト・配列を指定することもできます。


##### Dictionary<string,object> オブジェクト

`E.取得データ` の列名行に記述した名前をディクショナリのキー、
データ行に記述した予想結果値をディクショナリの値として検証を行うことができます。

Validate メソッドの引数には単体オブジェクトの他に、
Dictionary<string,object> オブジェクトのリスト・配列を指定することもできます。


##### DataTable

`E.取得データ` のテーブル定義名を DataTable のテーブル名 (DataTable.TableName)、
列名行に記述した名前を DataTable の列 (DataColumn.ColumnName)、
データ行に記述した予想結果値を DataRow の列値として検証を行うことができます

DataRow の列値が DBNull.Value の場合、検証時には null として扱われます。


##### DataSet

`E.取得データ` のテーブル定義名を DataTable のテーブル名 (DataTable.TableName)、
列名行に記述した名前を DataTable の列 (DataColumn.ColumnName)、
データ行に記述した予想結果値を DataRow の列値として検証を行うことができます。

DataRow の列値が DBNull.Value の場合、検証時には null として扱われます。

DataSet 内にある特定の DataTable のみを検証の対象としたい場合。
Validate メソッドの引数でテーブル定義名を明示的に指定してください。
その名前を持つ DataTable のみが検証の対象となります。

Validate の引数でテーブル定義名を明示的に指定しなかった場合は、
全ての DataTable が検証の対象になります。
この際、DataSet 内にある DataTable の数と
`E.取得データ` に記述されたテーブル定義の数とが異なる場合には、
エラーとなります。


##### string, int 等のオブジェクト

`E.取得データ` の列名行に `-` を指定すると、データ行に記述した予想結果値と比較検証することができます。

![string, int 等のオブジェクト](./images/FixtureBookReference-15.png?raw=true)

Validate メソッドの引数には単体オブジェクトの他に、リスト・配列を指定することもできます。


#### テーブル定義検索ルール

Validate メソッドでオブジェクトを検証する際に、
「`E.取得データ`にあるどのテーブル定義を利用するか」
については以下のルールで決定されます。

1.  `fixtureBook.Validate(obj, "テーブル定義名")` というふうにテーブル定義名を明示的に指定した場合は、
    その名前のテーブル定義を利用する。
2.  `fixtureBook.Validate(obj)` というふうにテーブル定義名が明示的に指定されなかった場合は、
    以下のルールが適用される。
    1.  同一テストケース、セクション内にテーブル定義がひとつしかなければそのテーブル定義が利用される。
    2.  テーブル定義が複数ある場合、引数のオブジェクトが DataTable もしくは DataSet だった場合、DataTable.TableName をテーブル定義名とみなす。
    3.  上記に当てはまらない場合、引数オブジェクトのクラス名をテーブル定義名とみなす。


#### 予想結果として記述できる値

以下の値が予想結果値として記述できます。

<table>
<tr>
  <th>記述できる値</th>
  <th>値の例</th>
  <th>値の意味</th>
</tr>
<tr>
  <td>任意の値</td>
  <td>abc</td>
  <td>検証する値の文字列表現</td>
</tr>
<tr>
  <td>何も書かない</td>
  <td></td>
  <td>null もしくは、長さゼロの文字列 ("")</td>
</tr>
<tr>
  <td>${NULL}</td>
  <td>${NULL}</td>
  <td>null</td>
</tr>
<tr>
  <td>${EMPTY}</td>
  <td>${EMPTY}</td>
  <td>長さゼロの文字列 ("")</td>
</tr>
<tr>
  <td>任意の値 + % (パーセント記号)</td>
  <td>abc%</td>
  <td>指定された文字列で始まるかどうか</td>
</tr>
<tr>
  <td>% + 任意の値</td>
  <td>%abc</td>
  <td>指定された文字列で終わるかどうか</td>
</tr>
<tr>
  <td>% + 任意の値 + %</td>
  <td>%abc%</td>
  <td>指定された文字列を含むかどうか</td>
</tr>
<tr>
  <td>${TODAY}</td>
  <td>${TODAY}</td>
  <td>本日の日付の範囲内かどうか</td>
</tr>
<tr>
  <td>`正規表現`</td>
  <td>`ab.?d`</td>
  <td>指定された正規表現にマッチするかどうか</td>
</tr>
</table>




### F.更新後データ

`F.更新後データ`には、テスト実行によって変更されたデータベーステーブルの状態予想 (期待結果) を記述します。

![F.更新後データ](./images/FixtureBookReference-10.png?raw=true)

列名の先頭に `*` がついた列の値を条件にしてデータベーステーブルの検索を行い、
取得した各列の値がセルに入力された値と一致しているかを検証します。
この「検索～検証」の処理は `F.更新後データ` のデータ行の行数分繰り返し行われます。

*   データベーステーブルの検索結果が2件以上の場合はエラーとなります。
*   `*` の列は複数指定できます。複数指定した場合は AND 条件での検索となります。
*   `*` のついた列がひとつもない場合、全ての列を検索条件として検索します。


#### 削除された行の扱い

![F.更新後データ](./images/FixtureBookReference-11.png?raw=true)

特定の行が削除されたことを検証する場合は、C列に `D` と書き、検索条件値を入力してください。


#### 単体テストプログラム内での利用方法

`FixtureBook.ValidateStorage()` を呼び出してください。
このメソッドを呼び出すと、現在のテストケースに関して
`F.更新後データ` に記述されているすべてのテーブル定義の検証が行われます。


```c#
    fixtureBook.ValidateStorage();
```


#### 予想結果として記述できる値

`E.取得データ` と同様に、 ${TODAY}, ${NULL}, ${EMPTY}, % による部分一致、\`...\` による正規表現が指定できます。




入力値の扱いに関する特記事項
----------------------------

ここでは、データ行のセルに入力する値に関しての特記事項を説明します。

### 数値とみなされる文字列

Excel というツールの特性上、先頭の "0" (ゼロ) は自動的に削除されてしまいます。
例えば、"009988" という文字列を指定したい場合にセルに "009988" と入力しても、
"9988" に変換されて Excel 内部では数値として保存されます。

この問題を回避するためには、セルの書式を「文字列」にしてください。

「数値が文字列として保存されています」というExcelの警告を気にしなければ、
最初にシート全体の書式を文字列にしておくのが最も安全で簡単な対処法です。


### 日時項目

Excel上で日時項目として扱われている値は、
FixtureBook の内部では、`yyyy-MM-dd HH:mm:ss` 形式の文字列に変換され、保持されています
（プロパティ型として DateTime が指定されている場合、その後 DateTime 型の値に変換されます）。


#### ミリ秒を指定したい場合

ミリ秒を扱う必要がある場合は、
`2012-12-03 12:34:56.123` というふうに
<b>yyyy-MM-dd HH:mm:ss.fff</b> 形式の文字列としてセルに入力を行ってください。


#### 時差指定がしたい場合

時差指定が必要な場合は
`2012-12-04 12:34:56.123456 +07:00` というふうに
<b>yyyy-MM-dd HH:mm:ss.fff +zzz</b> または
<b>yyyy-MM-dd HH:mm:ss.fff -zzz</b> の形式で
時差指定を行ってください。


### 配列

配列型の項目は、バーティカルバー区切りで値指定できます。

![配列指定](./images/FixtureBookReference-12.png?raw=true)


### バイト配列

バイト配列型 ( `byte[]` ) の項目は、以下のいずれかの方法で値指定できます。

*   ファイルパス指定 (指定されたファイルの読み込みが行われ、バイト配列として設定される)
*   Base64
*   バーティカルバー区切り


### 複合オブジェクト

データ行の値として別のテーブル定義名を指定することにより、
複合オブジェクトの初期化を行うことができます。

![複合オブジェクト](./images/FixtureBookReference-13.png?raw=true)

例えば、上記の定義を使って

```c#
    Order order = fixtureBook.GetObject<Order>();
```

とした場合と、以下のコード

```c#
    Order order = new Order
    {
        OrderNo = "H001",
        CustomerInfo = new Customer { Code = "C001", Name = "XX商事", Telno = "045-999-9999" },
        Detail = new List<OrderDetail> 
        { 
            new OrderDetail { DetailNo = "001", ItemCd = "X01", Qty = 10 },
            new OrderDetail { DetailNo = "002", ItemCd = "X02", Qty = 20 },
        }
    };
```

は、同じ状態のオブジェクトを取得できます。


#### 複合オブジェクト定義上の注意

Expect / ExpectReturn / ExpectThrown を使用する場合、
子テーブル定義はすべての親テーブル（子を持たないものもの含めて）の後に記述してください。
