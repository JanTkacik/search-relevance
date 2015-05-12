import csv
import re


def getwords(mystr):
    return re.sub("[^\w]", " ",  mystr).split()


def tolowercase(wordlist):
    return [x.lower() for x in wordlist]


def getpercentage(x, y):
    if y == 0:
        return 1
    return x / y

with open("C:\\Users\\jantk_000\\Documents\\SearchResultRelevance\\out.csv", 'r', encoding='utf-8') as f:
    with open('C:\\Users\\jantk_000\\Documents\\SearchResultRelevance\\extracted.csv', 'w', newline='') as csvfile:
        writer = csv.writer(csvfile, delimiter=',')
        # writer.writerow(["Id", "Relevance", "RelevanceVariance", "MatchedPercentage", "TitleMatchedPercentage", "DescriptionMatchedPercentage"])
        writer.writerow(["Id", "MatchedPercentage", "TitleMatchedPercentage", "DescriptionMatchedPercentage"])
        reader = csv.reader(f, delimiter=',', quotechar='"')
        reader.__next__()
        for row in reader:
            try:
                # idnum, query, title, desc, relm, relv = row
                idnum, query, title, desc = row
            except ValueError:
                print(len(row))
                print(row)
            querywords = tolowercase(getwords(query))
            titlewords = tolowercase(getwords(title))
            descwords = tolowercase(getwords(desc))
            titlematches = 0
            descmatches = 0
            matchedwords = set()
            for word in querywords:
                if word in titlewords:
                    titlematches += 1
                    matchedwords.add(word)
                if word in descwords:
                    descmatches += 1
                    matchedwords.add(word)
            matchedpercentage = getpercentage(len(matchedwords), len(querywords))
            titlematchedperc = getpercentage(titlematches, len(titlewords))
            descmatchedperc = getpercentage(descmatches, len(descwords))
            # writer.writerow([idnum, relm, relv, matchedpercentage, titlematchedperc, descmatchedperc])
            writer.writerow([idnum, matchedpercentage, titlematchedperc, descmatchedperc])


