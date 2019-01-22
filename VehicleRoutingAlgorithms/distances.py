from constants import *
from input_parser import *
from osrm_handling import *


# Set default distance calculator here
def calculate_distance(metric_id, position_1, position_2):
    if metric_id == OSRM_METRIC:
        return actual_distance(position_1, position_2)
    elif metric_id == MANHATTAN_METRIC:
        return manhattan_distance(position_1, position_2)
    else:
        print("Metric id {0} not recognized".format(metric_id))
        print_parse_error()
        sys.exit(0)


def manhattan_distance(position_1, position_2):
    return MANHATTAN_MULTIPLIER*(abs(position_1[0] - position_2[0]) + abs(position_1[1] - position_2[1]))


def actual_distance(position_1, position_2):
    return get_routing_distance_from_list_of_points([position_1, position_2])

