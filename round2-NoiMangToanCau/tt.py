#!/usr/bin/env python3
import random
import sys
from mapper import *
from string import printable

KEYWORD_LENGTH = 40

encrypted_chars = ' !"#$%&\'()*+,-./0123456789:;<=>?'

def decrypt(enc, k):
  assert len(enc) % 8 == 0, "Length of encrypted must be divisible by 8"
  tmp = ''
  res = ''
  bin_patch = []
  for c in enc:
    print(c, bin(ord(c))[2:])
    tmp += bin(ord(c))[3:]

  for i in range(0, len(tmp), 8):
    bin_patch.append(tmp[i:i+8])
    print(bin_patch[-1], bin(int(bin_patch[-1], 2) ^ k)[2:])
    c = chr(int(bin_patch[-1], 2) ^ k)
    assert c in printable, f"Invalid keyword character {c}, {str(ord(c))}"
    res += c

  return res, bin_patch

def encrypt(keyword, k):
  assert len(keyword) % 5 == 0, "Length of keyword must divsible by 5"
  tmp = ''
  res = ''
  bin_patch = []
  for c in keyword:
    tmp += bin(ord(c) ^ k)[2:].rjust(8, '0')

  for i in range(0, len(tmp), 5):
    bin_patch.append('1' + tmp[i:i+5])
    c = chr(int(bin_patch[-1], 2))
    assert c in encrypted_chars, f"Invalid encrypted character {c}, {str(ord(c))}"
    res += c

  return res, bin_patch

def map(c):
  if c not in mapper:
    return c

  l = len(mapper[c])
  return mapper[c][random.randint(0, l - 1)]

def gen(words, length):
  l = len(words)

  while True:
    chosen = words[random.randint(0, l - 1)]
    if len(chosen) < length // 1.5:
      continue
    chosen = chosen.split(" ")
    cur_words = []
    cur_l = 0
    for word in chosen:
      tmp = ""
      for c in word:
        tmp += map(c)
      
      cur_l += len(tmp) + (1 if len(cur_words) > 0 else 0)
      cur_words.append(tmp)

    if cur_l > length:
      continue

    remain = length - cur_l
    space_cnt = [1] * (len(cur_words) - 1)
    for i in range(remain):
      pos = random.randint(0, len(cur_words) - 2)
      space_cnt[pos] += 1

    res = ""
    for i in range(len(cur_words) - 1):
      res += cur_words[i]
      res += " " * space_cnt[i]

    res += cur_words[-1]
    return chosen, res

def main():
  if sys.argv[1] == '--enc':
    key = int(sys.argv[2])
    keyword = open("keyword", "r").read().strip()
    enc, _ = encrypt(keyword, key)
    print(f"Encrypted: [{enc}]")
  elif sys.argv[1] == '--dec':
    key = int(sys.argv[2])
    enc = open("encrypted", "r").read().strip()
    msg, _ = decrypt(enc, key)
    print(f"Decrypted: [{msg}]")
  else:
    words = open("words", "r").read().strip().split("\n")
    plain = ""
    mapped = ""
    while mapped == "":
      plain, mapped = gen(words, KEYWORD_LENGTH)

    print("Keyword:", plain)
    print("Mapped:", mapped)

    key = int(sys.argv[1])

    enc, bin_patch = encrypt(mapped, key)
    msg, bin_patch = decrypt(enc, key)
    # NOTE bin_patch just for debugging, meh

    # print(f"Encrypted: [{enc}]")
    # print(f"[DEBUG] Decrypted: [{msg}]")
    # print("[DEBUG] " + ("correct" if msg == mapped else "incorrect"))

if __name__ == "__main__":
  main()
