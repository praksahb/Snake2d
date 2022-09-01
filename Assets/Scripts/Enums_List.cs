public enum Direction
{
    left, up, right, down,
}

public enum SnakeType
{
    none, snakeHead, snakeBody
}

public enum SpawnType
{
    massGainer, massBurner, shieldBoost, scoreBoost, speedBoost,
}

public enum Player
{
    singlePlayer, playerLeft, playerRight, bluePlayer = playerLeft, blackPlayer = playerRight, none,
} 