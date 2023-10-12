using UnityEngine;

public class GameOfLife : MonoBehaviour
{
    public GameObject cellPrefab;
    public float cellSize = 0.1f;
    public int spawnChancePercentage = 20;

    // 1D array with vars to use it as 2D
    Cell[] cells;
    int columns, rows;

    void Start()
	{
		Application.targetFrameRate = 6;

        float screenWidth = Camera.main.orthographicSize * Camera.main.aspect * 2;
        float screenHeight = Camera.main.orthographicSize * 2;
        columns = Mathf.FloorToInt(screenWidth / cellSize);
        rows = Mathf.FloorToInt(screenHeight / cellSize);

        float offsetX = (screenWidth - (columns * cellSize)) / 2;
        float offsetY = (screenHeight - (rows * cellSize)) / 2;

        int cellGrid = columns * rows;
        cells = new Cell[cellGrid];

		for (int i = 0; i < cellGrid; i++)
		{
            // Formulas for defining the X and Y in the array
            int x = i % columns;
            int y = i / columns;

            Vector2 newPos = new Vector2(x * cellSize + offsetX - screenWidth / 2,
            y * cellSize + offsetY - screenHeight / 2);

            var newCell = Instantiate(cellPrefab, newPos, Quaternion.identity);

			newCell.transform.localScale = Vector2.one * cellSize;
			cells[i] = newCell.GetComponent<Cell>();

			if (Random.Range(0, 100) < spawnChancePercentage)
			{
				cells[i].alive = true;
			}

			cells[i].UpdateStatus();

            // Slightly change camera size to create a zoomed-out initial view
            Camera.main.orthographicSize = 5.5f;
        }
    }

    public int CellCount(int index)
    {
        int livingNeighbors = 0;

        // Formulas for defining the X and Y in the array
        int x = index % columns;
        int y = index / columns;

        // Check all eight neighbors for cells
        for (int i = x - 1; i <= x + 1; i++)
        {
            for (int j = y - 1; j <= y + 1; j++)
            {
                // Ensure that cells don't count themselves as a neighbor
                if (i == x && j == y)
                    continue;

                int neighborX = (i + columns) % columns;
                int neighborY = (j + rows) % rows;
                int neighborI= neighborY * columns + neighborX;

                // Don't check out of the bounds of the array
                if (i >= 0 && i < columns && j >= 0 && j < rows)
                {
                    if (cells[neighborI].alive)
                    {
                        livingNeighbors++;
                    }
                }
            }
        }
        return livingNeighbors;
    }

    private Vector3 lastMousePosition;

    public void CameraControls()
    {
        float zoomSpeed = 1f;

        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");

        bool isRightMouseButtonDown = Input.GetMouseButton(1);

        if (scrollWheel < 0f)
        {
            Camera.main.orthographicSize += zoomSpeed;
        }
        else if (scrollWheel > 0f)
        {
            // Store the mouse position before zooming
            Vector3 mousePositionBeforeZoom = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Camera.main.orthographicSize -= zoomSpeed;

            // Zoom towards the mouse cursor
            Vector3 mousePositionAfterZoom = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 mouseDelta = mousePositionBeforeZoom - mousePositionAfterZoom;
            Camera.main.transform.position += mouseDelta;
        }
        else if (isRightMouseButtonDown)
        {
            // Pan the camera while holding the right mouse button
            Vector3 mouseDelta = lastMousePosition - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Camera.main.transform.position += mouseDelta;
        }

        // Update the last mouse position regardless of zooming
        lastMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }


    void Update()
	{
        CameraControls();

        int cellGrid = columns * rows;
        bool[] nextGeneration = new bool[cellGrid];

        for (int i = 0; i < cellGrid; i++)
        {
            int livingNeighbors = CellCount(i);
            bool isAlive = cells[i].alive;

            // The rules of Conway's Game of Life
            if (isAlive)
            {
                if (livingNeighbors < 2)
                {
                    nextGeneration[i] = false;
                }

                else if (livingNeighbors == 2 || livingNeighbors == 3)
                {
                    nextGeneration[i] = true;
                }

                else if (livingNeighbors >= 4)
                {
                    nextGeneration[i] = false;
                }
            }
            else // Dead cells
            {
                if (livingNeighbors == 3)
                {
                    nextGeneration[i] = true;
                }
                else
                {
                    nextGeneration[i] = false;
                }
            }
        }

        for (int i = 0; i < cellGrid; i++)
		{
            cells[i].alive = nextGeneration[i];
            cells[i].UpdateStatus();
		}
	}
}
