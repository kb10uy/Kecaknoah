# case文(switch)のサンプル
# 構文木ではif文になるのであまり使用は
# おすすめできないですが、可読性の上では
# まあいいんじゃないでしょうか。
func main
  case(1)
    when 1:
      println(1)
    when 2:
      println(1)
    when 3:
    when 4:
    when 5:
    when 6:
      println("> 2")
    default:
      println("?")
  endcase
endfunc