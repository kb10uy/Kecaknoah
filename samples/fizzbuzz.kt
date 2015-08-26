func main
  for(i = 1; i <= 100; i++)
    if (i % 15 == 0) then
      println("FizzBuzz")
    elif (i % 5 == 0) then
      println("Buzz")
    elif (i % 3 == 0) then
      println("Fizz")
    else
      println(i)
    endif
  next
endfunc