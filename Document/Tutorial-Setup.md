
### チュートリアル#01

セットアップ
============

セットアップは以下の手順で行います。

1.  単体テストプロジェクトを作成する。
2.  作成した単体テストプロジェクトの参照設定で「NuGet パッケージの管理」を行い、
    [FixtureBook](https://www.nuget.org/packages/FixtureBook/) をインストールする。
3.  作成した単体テストプロジェクトで、テスト対象のプロジェクトに対して参照設定を行う。
4.  作成した単体テストプロジェクトの中に App.config を作成し、下記のようにデータベース接続設定を行う。


データベース接続設定
--------------------

データベース接続設定は以下のように記述します。

```xml
    <?xml version="1.0" encoding="utf-8" ?>
    <configuration>
      <connectionStrings>
        <clear/>
        <add name="SQLServer" providerName="System.Data.SqlClient" connectionString="Data Source=(LocalDB)\v11.0;integrated security=True;Initial Catalog=test1"/>
      </connectionStrings>
    </configuration>
```

*   &lt;connectionStrings&gt; の中に &lt;add .../&gt; が複数記述されている場合は一番最初に記述されている定義が使用されます。
*   &lt;connectionStrings&gt; の先頭に &lt;clear/&gt; を入れない場合、machine.config の一番最初に記述されている定義が使用されます。


### 複数の接続先を利用する場合

一つの単体テストから複数のデータベースにアクセスする必要がある場合は、デフォルトの接続先を最初に定義してください。
例えば、以下のように定義した場合は、SQLServer がデフォルト接続先として使用されます。

```xml
    <?xml version="1.0" encoding="utf-8" ?>
    <configuration>
      <connectionStrings>
        <clear/>
        <add name="SQLServer" providerName="System.Data.SqlClient" connectionString="Data Source=(LocalDB)\v11.0;integrated security=True;Initial Catalog=test1"/>
        <add name="Oracle" providerName="Oracle.DataAccess.Client" connectionString="User Id=system;Password=manager;Data Source=xe"/>
      </connectionStrings>
    </configuration>
```

上記の設定をしている際に Oracle 接続を使用したい場合は、
FixtureBook のテーブル名指定箇所に `[テーブル名]@[接続名]` という形式で接続名の指定を行います。

![テーブル記述](./images/Tutorial-Setup-01.png?raw=true)



デバッグログ出力設定
--------------------

以下の設定 ( &lt;system.diagnostics&gt; ～ &lt;/system.diagnostics&gt; ) を追加すると、
FixtureBook の処理中に実行された SQL 文やデータベース接続情報がデバッグ出力されるようになります。

```xml
    <?xml version="1.0" encoding="utf-8"?>
        <configuration>
          ...
          <system.diagnostics>
            <switches>
              <add name="XPFriend" value="4"/>
            </switches>
          </system.diagnostics>
          ...
        </configuration>
```


------------------------

*   [チュートリアル#02 - Entity Framework コードファースト開発での利用例](./Tutorial-CodeFirst.md)
*   [チュートリアル#03 - DataSet / DataTableを使った開発での利用例](./Tutorial-DataSet.md)

