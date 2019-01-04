from __future__ import print_function
from ortools.constraint_solver import pywrapcp
from ortools.constraint_solver import routing_enums_pb2
from distances import *
from osrmHandling import *

def createDistanceCallback(metricID, data):
    """Creates callback to return distance between points."""
    _distances = {}

    for from_node in range(len(data)):
        _distances[from_node] = {}
        for to_node in range(len(data)):
            if from_node == to_node:
                _distances[from_node][to_node] = 0
            else:
                _distances[from_node][to_node] = (
                    calculateDistance(metricID, data[from_node], data[to_node]))

    def distanceCallback(from_node, to_node):
        """Returns the distance between the two nodes"""
        return _distances[from_node][to_node]

    return distanceCallback


def getSolution(coords, metricID, routing, assignment):
    index = routing.Start(0)
    solution = []
    distance = 0
    while not routing.IsEnd(index):
        solution.append(routing.IndexToNode(index))
        previous_index = index
        index = assignment.Value(routing.NextVar(index))
        distance += routing.GetArcCostForVehicle(previous_index, index, 0)
    points = []
    for sol in solution:
        points.append([coords[sol][0], coords[sol][1]])
    if metricID != OSRM_METRIC:
        distance = getRoutingDistanceFromListOfPoints(points)

    return solution, distance, points

def printSolutionOnConsole(solution, distance, t):
    output = 'Route for vehicle'
    for item in solution[0:len(solution)]:
        output += ' {} ->'.format(item)
    output += ' {0}'.format(solution[0])
    output += ' \nDistance of route: {0}m\nTime: {1}\n'.format(distance, t)
    print(output)


def saveToOutputFile(vehicleID, points, overallDistance, t):
    listOfStrings = []
    for p in points:
        listOfStrings.append(str(p).replace('[', '').replace(']', '').replace(' ', '')+'\n')
    listOfStrings.append(str(overallDistance) + '\n')
    listOfStrings.append(str(t))

    with open('output' + str(vehicleID) + '.txt', 'w') as file:
        file.writelines(listOfStrings)

def addDistanceDimension(routing, distance_callback):
    """Add Global Span constraint"""
    distance = 'Distance'
    maximum_distance = 1000000  # Maximum distance per vehicle - 1000 km.
    routing.AddDimension(
        distance_callback,
        0,  # null slack
        maximum_distance,
        True,
        distance)
    distance_dimension = routing.GetDimensionOrDie(distance)
    distance_dimension.SetGlobalSpanCostCoefficient(100)
