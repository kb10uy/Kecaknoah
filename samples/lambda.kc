# ラムダ式 サンプル
# Kecaknoahではラムダ式は単一式のみ利用でき、
# &(arg1, arg2, ...) => expression
# の形式で記述します。
# もちろんメソッドの引数などに渡すことも可能です。
func main
  # ラムダ式は実行可能オブジェクトなので直接呼び出せます
  println("ラムダ式の直接実行")
  println((&(a,b)=>a * b)(10, 20))
  # あとで実装する予定の配列・リストのmapメソッド例
  println("述語としてのラムダ式の利用")
  list = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
  list2 = map(list,&(p)=>p * 2)
  for(i = 0; i < list2.length; i++)
    println(list2[i])
  next
endfunc

func map(arr, acm)
  ret = array(arr.length)
  for(i = 0; i < arr.length; i++)
    ret[i] = acm(arr[i])
  next
  return ret
endfunc
