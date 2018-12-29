from __future__ import print_function
from solver import *
import sys
import timeit

def findRouting(coords):

    start = timeit.default_timer()

    # Create Routing Model
    routing = pywrapcp.RoutingModel(len(coords), 1, 0)

    # Define weight of each edge
    distance_callback = createDistanceCallback(coords)
    routing.SetArcCostEvaluatorOfAllVehicles(distance_callback)
    addDistanceDimension(routing, distance_callback)

    # Setting first solution heuristic (cheapest addition).
    search_parameters = pywrapcp.RoutingModel.DefaultSearchParameters()
    search_parameters.first_solution_strategy = routing_enums_pb2.FirstSolutionStrategy.PATH_CHEAPEST_ARC

    # Solve the problem.
    assignment = routing.SolveWithParameters(search_parameters)
    if assignment:
        solution, overallDistance = getSolution(routing, assignment)
        stop = timeit.default_timer()
        t = stop - start
        printSolutionOnConsole(solution, overallDistance, t)
        saveToOutputFile(vehicleID, coords, solution, overallDistance, t)


if __name__ == '__main__':
    vehicleID = int(sys.argv[1])
    with open('input' + str(vehicleID) + '.txt') as f:
        locations = [tuple(map(float, i.split(','))) for i in f]

    findRouting(locations)
