import requests
import json
import sys
from constants import *
from inputParser import *
from osrmHandling import *

# Set default distance calculator here
def calculateDistance(metricID, position_1, position_2):
    if metricID == OSRM_METRIC:
        return actualDistance(position_1, position_2)
    elif metricID == MANHATTAN_METRIC:
        return manhattanDistance(position_1, position_2)
    else:
        print("Metric id {0} not recognized".format(metricID))
        printParseError()
        sys.exit(0)

def manhattanDistance(position_1, position_2):
    return abs(position_1[0] - position_2[0]) + abs(position_1[1] - position_2[1])

def actualDistance(position_1, position_2):
    return getRoutingDistanceFromListOfPoints([position_1, position_2])

