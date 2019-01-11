import itertools
from osrmHandling import *
import timeit

def bruteForce(locations):
    start = timeit.default_timer()
    permutations = list(itertools.permutations(locations[1:]))
    dist = 2 * getRoutingDistanceFromListOfPoints(permutations[0])
    for p in permutations:
        spawn = (locations[0],)
        route = spawn + p
        dist_n = getRoutingDistanceFromListOfPoints(route)
        if dist_n < dist:
            dist = dist_n
            ret_route = route
    print("Distance of route: {0}m".format(dist))
    print("Time: {0}".format(timeit.default_timer() - start))
        