# Set default distance calculator here
def calculateDistance(position_1, position_2):
    return manhattanDistance(position_1, position_2)


def manhattanDistance(position_1, position_2):
    return abs(position_1[0] - position_2[0]) + abs(position_1[1] - position_2[1])

# TODO: other metrics
