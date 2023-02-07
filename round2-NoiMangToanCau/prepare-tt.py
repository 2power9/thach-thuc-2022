#!/usr/bin/env python3

import requests
import re

def sanitize(s):
  s = s.lower()
  s = re.sub("\.", ",", s)
  s = re.sub("\/", " ", s)
  s = re.sub("à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ", "a", s)
  s = re.sub("è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ", "e", s)
  s = re.sub("ì|í|ị|ỉ|ĩ", "i", s); 
  s = re.sub("ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ", "o", s); 
  s = re.sub("ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ", "u", s); 
  s = re.sub("ỳ|ý|ỵ|ỷ|ỹ", "y", s); 
  s = re.sub("đ", "d", s);
  return s

r = requests.get("https://vi.wikiquote.org/wiki/Th%C3%A0nh_ng%E1%BB%AF_Vi%E1%BB%87t_Nam")
res = []
for match in re.findall('<li>.+</li>', r.text):
  match = match[4:-5]
  if match[-1] == ".":
    match = match[:-1]
  res.append(sanitize(match))

open("words", "w").write('\n'.join(res))
