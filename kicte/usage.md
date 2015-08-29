Kicte - Kecaknoah Interop Class Template Engine
================================================================

# 使い方
* このkicteディレクトリの上にKecakc.exeを配置します。
 - バイナリパッケージでは特に何もする必要はありません
* このディレクトリに好きな名前でテキストファイルを作成します。
* 後述の文法にしたがって記述します。
* 好きなだけ作ったらgenerate.batを実行します。
* generatedディレクトリにcsファイルが生成されているのでそれに処理を追加し、コンパイルします。

# 文法
1ファイルに付き1クラス分の情報を記述する。  
メソッドとフィールドのコードしか生成しないのでインデクサなどは手動でオーバーライドする必要あり。
## サンプル
```
ClassName = Sample
HasConstructor = true
Fields = hoge:string, *huga:int
InstanceMethods = inst_method:void, inst_method2:int
ClassMethods = class_method:bool
```

## ClassName ディレクティブ
```
ClassName = <クラス名>
```
* クラス名を指定する。
* Kecaknoah内でのクラス名がココで指定したクラス名になる。
* 生成されるコードでのクラス名は先頭にKecaknoahが付与される。

## HasConstructor ディレクティブ
```
HasConstructor = (true|false)
```
* コンストラクタのクラスメソッドを生成する場合はtrueを、しない場合はfalseを指定する。
* 指定しない場合はtrue。

## Fields ディレクティブ
```
Fields = name:type, name2:type...
```
* 生成するフィールドを記述する。
* typeの部分には、以下のものが指定できる。省略した場合はint。
 - bool
 - int
 - float
 - string
 - object
* `*name:type`のように、先頭に*を付与した場合そのフィールドが左辺値扱いになる。
* 型は動的なので型を指定しても初期値の型にしか影響しない。

## InstanceMethods ディレクティブ
```
InstanceMethods = name:type, name2:type...
```
* 生成するインスタンスメソッドを記述する。
* typeの部分には、以下のものが指定できる。省略した場合はvoid。
 - void (KecaknoahNil)
 - bool (KecaknoahBoolean)
 - int (KecaknoahInteger)
 - float (KecaknoahFloat)
 - string (KecaknoahString)
 - object (KecaknoahObject)
 - array (KecaknoahArray)
* result変数を返り値にするコードが生成される。
* 型は動的なので型を指定しても初期値の型にしか影響しない。

## ClassMethods ディレクティブ
```
ClassMethods = name:type, name2:type...
```
* 生成するクラスメソッドを記述する。
* それ以外はInstanceMethodsディレクティブにならう。