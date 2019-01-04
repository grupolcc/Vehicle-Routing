from __future__ import print_function
from solver import *
from distances import *
from inputParser import *
from constants import *
import sys
import timeit

def findRouting(coords):

    start = timeit.default_timer()

    # Create Routing Model
    routing = pywrapcp.RoutingModel(len(coords), 1, 0)

    # Define weight of each edge
    distance_callback = createDistanceCallback(metricID, coords)
    routing.SetArcCostEvaluatorOfAllVehicles(distance_callback)
    addDistanceDimension(routing, distance_callback)

    # Setting first solution heuristic (cheapest addition).
    search_parameters = pywrapcp.RoutingModel.DefaultSearchParameters()
    search_parameters.first_solution_strategy = routing_enums_pb2.FirstSolutionStrategy.PATH_CHEAPEST_ARC

    # Solve the problem.
    assignment = routing.SolveWithParameters(search_parameters)
    if assignment:
        solution, overallDistance, points = getSolution(coords, metricID, routing, assignment)
        t = timeit.default_timer() - start
        printSolutionOnConsole(solution, overallDistance, t)
        saveToOutputFile(vehicleID, points, overallDistance, t)

if __name__ == '__main__':
    vehicleID, metricID = parseInput()
    with open('input' + str(vehicleID) + '.txt') as f:
        locations = [tuple(map(float, i.split(','))) for i in f]

    findRouting(locations)