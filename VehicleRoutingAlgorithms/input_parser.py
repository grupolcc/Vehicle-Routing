import sys
from constants import *


def print_parse_error():
    print("Usage: main.py inputFile [algorithm_ID]")
    print("Example: main.py input2.txt")
    print("Example: main.py input3.txt 1")


def parse_input():
    if len(sys.argv) < 2 or len(sys.argv) > 3:
        print_parse_error()
        sys.exit(0)
    try:
        inputFile = str(sys.argv[1])
        if len(sys.argv) == 3:
            algorithm_ID = int(sys.argv[2])
        else:
            algorithm_ID = OSRM_ALGORITHM
    except ValueError:
        print_parse_error()
        sys.exit(0)
    return inputFile, algorithm_ID
