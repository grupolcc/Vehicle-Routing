import sys
from constants import *

def printParseError():
    print("Usage: main.py vehicleID [metricID]")
    print("Example: main.py 2")
    print("Example: main.py 3 1")

def parseInput():
    if len(sys.argv) < 2 or len(sys.argv) > 3:
        printParseError()
        sys.exit(0)
    try:
        vehicleID = int(sys.argv[1])
        if len(sys.argv) == 3:
            metricID = int(sys.argv[2])
        else:
            metricID = OSRM_METRIC
    except ValueError:
        printParseError()
        sys.exit(0)
    return vehicleID, metricID