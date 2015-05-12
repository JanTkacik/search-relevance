with open("C:\\Users\\jantk_000\\Documents\\SearchResultRelevance\\test.csv", 'r', encoding='utf-8') as f:
    with open("C:\\Users\\jantk_000\\Documents\\SearchResultRelevance\\out.csv", 'w', encoding='utf-8') as o:
        prev = ','
        quoted = False
        while True:
            c = f.read(1)
            if not c:
                break
            if c == '\n':
                if quoted:
                    continue
            if c == ',':
                if quoted:
                    continue
            if not c == '"':
                o.write(c)
                prev = c
                continue
            else:
                if prev == ',':
                    quoted = True
                    o.write(c)
                    prev = c
                    continue
                else:
                    n = f.read(1)
                    if n == ',' or n == "\n":
                        quoted = False
                        o.write(c)
                        o.write(n)
                        prev = n
                        continue
