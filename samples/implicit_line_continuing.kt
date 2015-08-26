func main
  for(
    i = 0;
    i < 9;
    i++
  )
    println(
      i
    )
  next
  foreach(
    x
    of
    three:(
      "ココアちゃんの子宮",
      "チノちゃんのおまんこ",
      "リゼちゃんのおっぱい"
    )
  ) println(x)
endfunc

func three(a, b, c)
  yield a
  yield b
  yield c
endfunc