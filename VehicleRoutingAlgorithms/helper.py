import re

def pretty_point(point):
    return re.sub("[\[\]()\s]", '', str(point)) + '\n'
