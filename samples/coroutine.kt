func main
  # coroutine name = coroutine_function_name
  # でコルーチンを起動します
  coroutine c = cor_test;
  # coresume(name)で次のyieldまですすめ、返り値があれば返します。
  # 終了してこれ以上値を返せない場合はnilが帰ります。
  while((x = coresume(c)) != nil)
    println(x)
  next
endfunc

func cor_test
  list = get_gochiusa()
  for(i = 0; i < list.length; i++)
    yield list[i]
  next
endfunc

func get_gochiusa
  gochiusa = array(5)
  gochiusa[0] = "保登心愛"
  gochiusa[1] = "香風智乃"
  gochiusa[2] = "天々座理世"
  gochiusa[3] = "宇治松千夜"
  gochiusa[4] = "桐間紗路"
  return gochiusa
endfunc