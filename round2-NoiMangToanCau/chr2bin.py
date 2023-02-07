from random import randint

def main():
  v = randint(32, 63)
  b = bin(v)[3:]
  c = chr(v)
  print(c)
  i = input()
  if i == b:
    print("Correct")
  else:
    print("Wrong!")
    print(b)

if __name__ == "__main__":
  while True:
    main()
