from __future__ import print_function
from ortools.constraint_solver import pywrapcp
from ortools.constraint_solver import routing_enums_pb2
from distances import *


def createDistanceCallback(data):
    """Creates callback to return distance between points."""
    _distances = {}

    for from_node in range(len(data)):
        _distances[from_node] = {}
        for to_node in range(len(data)):
            if from_node == to_node:
                _distances[from_node][to_node] = 0
            else:
                _distances[from_node][to_node] = (
                    calculateDistance(data[from_node], data[to_node]))

    def distanceCallback(from_node, to_node):
        """Returns the distance between the two nodes"""
        return _distances[from_node][to_node]

    return distanceCallback


def getSolution(routing, assignment):
    index = routing.Start(0)
    solution = []
    distance = 0
    while not routing.IsEnd(index):
        solution.append(routing.IndexToNode(index))
        previous_index = index
        index = assignment.Value(routing.NextVar(index))
        distance += routing.GetArcCostForVehicle(previous_index, index, 0) # TODO: calculate the actual distance not the latitude/longitude one
    solution.append(routing.IndexToNode(index))
    return solution, distance


def printSolutionOnConsole(solution, distance):
    """Print route on console"""
    output = 'Route for vehicle'
    for item in solution[1:len(solution)-1]:
        output += ' {} ->'.format(item)
    output += ' {0}\nDistance of route: {1}m\n'.format(solution[len(solution) - 1], distance)
    print(output)


def saveToOutputFile(vehicleID, coords, solution, overallDistance):
    listOfStrings = []
    for sol in solution:
        listOfStrings.append(str(coords[sol]).replace('(', '').replace(')', '').replace(' ', '')+'\n')
    listOfStrings.append(str(overallDistance))

    with open('output' + str(vehicleID) + '.txt', 'w') as file:
        file.writelines(listOfStrings)


def addDistanceDimension(routing, distance_callback):
    """Add Global Span constraint"""
    distance = 'Distance'
    maximum_distance = 25000  # Maximum distance per vehicle - 25 km.
    routing.AddDimension(
        distance_callback,
        0,  # null slack
        maximum_distance,
        True,
        distance)
    distance_dimension = routing.GetDimensionOrDie(distance)
    distance_dimension.SetGlobalSpanCostCoefficient(100)
