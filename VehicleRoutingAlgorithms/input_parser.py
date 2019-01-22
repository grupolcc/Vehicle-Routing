import sys
from constants import *


def print_parse_error():
    print("Usage: main.py vehicleID [metricID]")
    print("Example: main.py 2")
    print("Example: main.py 3 1")


def parse_input():
    if len(sys.argv) < 2 or len(sys.argv) > 3:
        print_parse_error()
        sys.exit(0)
    try:
        vehicleID = int(sys.argv[1])
        if len(sys.argv) == 3:
            metricID = int(sys.argv[2])
        else:
            metricID = OSRM_METRIC
    except ValueError:
        print_parse_error()
        sys.exit(0)
    return vehicleID, metricID
