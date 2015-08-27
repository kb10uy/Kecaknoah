func main
  println("List サンプル----------------")
  list_test()
  
  println()
  println("Dictionary サンプル----------")
  dictionary_test()
endfunc


# list サンプル
# .NETのそれのサブセット
func list_test
  list = List.new()
  list.add("kb10uy")
  list.add("h1manoa")
  list.add("gaogao_9")
  
  foreach(sn in list) println(sn)
endfunc

# dictionary サンプル
# キーの値はなんでもOK
# ただしdictionary自体に対するforeachは
# 超限定的にしか利用できないので注意
func dictionary_test
  dict = Dictionary.new()
  dict["佐倉綾音"] = ["友利奈緒", "越谷夏海", "保登心愛"]
  dict["田村ゆかり"] = 17
  
  println(format("田村ゆかり{0}歳",dict["田村ゆかり"]))
  foreach(chara in dict["佐倉綾音"]) println(chara)
endfunc