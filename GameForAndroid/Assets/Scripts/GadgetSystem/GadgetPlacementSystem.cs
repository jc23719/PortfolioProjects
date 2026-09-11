using UnityEngine;
using UnityEngine.Tilemaps;

public class GadgetPlacementSystem : MonoBehaviour
{
    public Camera cam;
    public Tilemap tilemap;
    public GameObject stickyWallPrefab;
    public GameObject stickyWallGhostPrefab;
    public GameObject jumpPadPrefab;
    public GameObject jumpPadGhostPrefab;
    public GameObject fanPrefab;
    public GameObject fanGhostPrefab;
    public GameObject plugPrefab;
    public GameObject plugGhostPrefab;

    private bool placingPlug = false;
    private bool placingFan = false;
    private GameObject currentGhost;
    private bool placingStickyWall = false;
    private bool placingJumpPad = false;
    private bool isDragging = false;
    private bool validPlacement = false;

    private Grid grid;
    public Collider2D plugTriggerCollider;

    private enum SurfaceType { Floor, Ceiling, LeftWall, RightWall, None }

    void Start()
    {
        grid = FindObjectOfType<Grid>();
    }

    void Update()
    {
        if (!placingStickyWall && !placingJumpPad && !placingFan && !placingPlug)
            return;

        Vector2 worldPos = GetPointerWorldPosition();

        if (currentGhost == null)
        {
            Vector2 centerScreen = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Vector2 centerWorld = cam.ScreenToWorldPoint(centerScreen);

            if (placingStickyWall)
                currentGhost = Instantiate(stickyWallGhostPrefab, centerWorld, Quaternion.identity);
            if (placingJumpPad)
                currentGhost = Instantiate(jumpPadGhostPrefab, centerWorld, Quaternion.identity);
            if (placingFan)
                currentGhost = Instantiate(fanGhostPrefab, centerWorld, Quaternion.identity);
            if (placingPlug)
                currentGhost = Instantiate(plugGhostPrefab, centerWorld, Quaternion.identity);

            return;
        }

#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
            isDragging = true;
#else
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            isDragging = true;
#endif

        if (!isDragging)
            return;

        Collider2D hit = Physics2D.OverlapPoint(worldPos);

        if (hit != null)
        {
            SurfaceType surface = GetSurface(worldPos);
            if (placingFan)
            {
                bool isValidSurface =
                    surface == SurfaceType.LeftWall ||
                    surface == SurfaceType.RightWall ||
                    surface == SurfaceType.Floor ||
                    surface == SurfaceType.Ceiling;

                if (!isValidSurface)
                {
                    currentGhost.transform.position = worldPos;
                    currentGhost.GetComponent<GadgetGhost>().SetValid(false);
                    validPlacement = false;
                }
                else
                {
                    Vector2 pos = SnapToSurface(worldPos, surface);
                    SpriteRenderer sr = currentGhost.GetComponentInChildren<SpriteRenderer>();

                    if (sr != null)
                    {
                        if (surface == SurfaceType.Floor)
                            pos.y -= sr.bounds.extents.y;
                        if (surface == SurfaceType.Ceiling)
                            pos.y += sr.bounds.extents.y;
                        if (surface == SurfaceType.LeftWall)
                            pos.x += sr.bounds.extents.x;
                        if (surface == SurfaceType.RightWall)
                            pos.x -= sr.bounds.extents.x;
                    }

                    currentGhost.transform.position = pos;

                    switch (surface)
                    {
                        case SurfaceType.LeftWall:
                            currentGhost.transform.rotation = Quaternion.Euler(0, 0, 180);
                            break;
                        case SurfaceType.RightWall:
                            currentGhost.transform.rotation = Quaternion.Euler(0, 0, 0);
                            break;
                        case SurfaceType.Floor:
                            currentGhost.transform.rotation = Quaternion.Euler(0, 0, 90);
                            break;
                        case SurfaceType.Ceiling:
                            currentGhost.transform.rotation = Quaternion.Euler(0, 0, -90);
                            break;
                    }

                    currentGhost.GetComponent<GadgetGhost>().SetValid(true);
                    validPlacement = true;
                }
            }


            if (placingStickyWall)
            {
                bool isWall = surface == SurfaceType.LeftWall || surface == SurfaceType.RightWall;

                if (!isWall)
                {
                    currentGhost.transform.position = worldPos;
                    currentGhost.GetComponent<GadgetGhost>().SetValid(false);
                    validPlacement = false;
                }
                else
                {
                    Vector2 pos = SnapToSurface(worldPos, surface);
                    SpriteRenderer sr = currentGhost.GetComponentInChildren<SpriteRenderer>();

                    if (sr != null)
                    {
                        if (surface == SurfaceType.LeftWall)
                            pos.x += sr.bounds.extents.x;
                        if (surface == SurfaceType.RightWall)
                            pos.x -= sr.bounds.extents.x;
                    }

                    currentGhost.transform.position = pos;

                    currentGhost.transform.rotation =
                        surface == SurfaceType.LeftWall ?
                        Quaternion.Euler(0, 0, 180) :
                        Quaternion.Euler(0, 0, 0);

                    currentGhost.GetComponent<GadgetGhost>().SetValid(true);
                    validPlacement = true;
                }
            }

            if (placingJumpPad)
            {
                bool isFloor = surface == SurfaceType.Floor;

                if (!isFloor)
                {
                    currentGhost.transform.position = worldPos;
                    currentGhost.GetComponent<GadgetGhost>().SetValid(false);
                    validPlacement = false;
                }
                else
                {
                    Vector2 pos = SnapToSurface(worldPos, surface);
                    SpriteRenderer sr = currentGhost.GetComponentInChildren<SpriteRenderer>();
                    if (sr != null)
                        pos.y -= sr.bounds.extents.y;

                    currentGhost.transform.position = pos;
                    currentGhost.transform.rotation = Quaternion.identity;
                    currentGhost.GetComponent<GadgetGhost>().SetValid(true);
                    validPlacement = true;
                }
            }

            if (placingPlug)
            {
                Debug.Log("Pointer: " + worldPos + " | Trigger: " + plugTriggerCollider.transform.position);

                float dist = Vector2.Distance(worldPos, plugTriggerCollider.transform.position);

                if (dist < 0.5f)
                {
                    currentGhost.transform.position = plugTriggerCollider.transform.position;
                    currentGhost.GetComponent<GadgetGhost>().SetValid(true);
                    validPlacement = true;
                }
                else
                {
                    currentGhost.transform.position = worldPos;
                    currentGhost.GetComponent<GadgetGhost>().SetValid(false);
                    validPlacement = false;
                }
            }
        }
        else
        {
            currentGhost.transform.position = worldPos;
            currentGhost.GetComponent<GadgetGhost>().SetValid(false);
            validPlacement = false;
        }

#if UNITY_EDITOR
        if (Input.GetMouseButtonUp(0))
            FinishPlacement(hit);
#else
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            FinishPlacement(hit);
#endif
    }

    private void FinishPlacement(Collider2D hit)
    {
        if (validPlacement)
        {
            if (placingStickyWall)
            {
                if (InventoryManager.Instance.UseItem(GadgetType.StickyWall))
                    Instantiate(stickyWallPrefab, currentGhost.transform.position, currentGhost.transform.rotation);
                else
                {
                    CancelPlacement();
                    return;
                }
            }

            if (placingJumpPad)
            {
                if (InventoryManager.Instance.UseItem(GadgetType.JumpPad))
                    Instantiate(jumpPadPrefab, currentGhost.transform.position, currentGhost.transform.rotation);
                else
                {
                    CancelPlacement();
                    return;
                }
            }

            if (placingFan)
            {
                if (InventoryManager.Instance.UseItem(GadgetType.Fan))
                {
                    GameObject fan = Instantiate(fanPrefab, currentGhost.transform.position, currentGhost.transform.rotation);

                    bool isWall = Mathf.Abs(hit.transform.up.x) > 0.7f;
                    fan.GetComponent<Fan>().isWallFan = isWall;

                    Vector2 dir = fan.transform.right;
                    fan.GetComponent<Fan>().pushDirection = dir;
                }
                else
                {
                    CancelPlacement();
                    return;
                }
            }

            if (placingPlug)
            {
                if (InventoryManager.Instance.UseItem(GadgetType.Plug))
                    Instantiate(plugPrefab, currentGhost.transform.position, Quaternion.identity);
                else
                {
                    CancelPlacement();
                    return;
                }
            }
        }

        Destroy(currentGhost);
        currentGhost = null;
        placingStickyWall = false;
        placingJumpPad = false;
        placingFan = false;
        placingPlug = false;
        isDragging = false;
        validPlacement = false;

        GadgetUI.Instance.RefreshUI();
    }

    private Vector2 GetPointerWorldPosition()
    {
#if UNITY_EDITOR
        return cam.ScreenToWorldPoint(Input.mousePosition);
#else
        if (Input.touchCount > 0)
            return cam.ScreenToWorldPoint(Input.GetTouch(0).position);
        return Vector2.zero;
#endif
    }

    private Vector2 SnapToSurface(Vector2 worldPos, SurfaceType surface)
    {
        Vector3Int cell = grid.WorldToCell(worldPos);
        Vector3 center = grid.GetCellCenterWorld(cell);
        Vector3 size = grid.cellSize;

        switch (surface)
        {
            case SurfaceType.Floor:
                return new Vector2(center.x, center.y + size.y / 2f);
            case SurfaceType.Ceiling:
                return new Vector2(center.x, center.y - size.y / 2f);
            case SurfaceType.LeftWall:
                return new Vector2(center.x - size.x / 2f, center.y);
            case SurfaceType.RightWall:
                return new Vector2(center.x + size.x / 2f, center.y);
        }

        return center;
    }

    private SurfaceType GetSurface(Vector2 worldPos)
    {
        Vector3Int cell = grid.WorldToCell(worldPos);

        bool hasTile      = TileExists(cell);
        bool tileBelow    = TileExists(cell + new Vector3Int(0, -1, 0));
        bool tileAbove    = TileExists(cell + new Vector3Int(0, 1, 0));
        bool tileLeft     = TileExists(cell + new Vector3Int(-1, 0, 0));
        bool tileRight    = TileExists(cell + new Vector3Int(1, 0, 0));

        bool emptyBelow   = !tileBelow;
        bool emptyAbove   = !tileAbove;
        bool emptyLeft    = !tileLeft;
        bool emptyRight   = !tileRight;

        if (hasTile && emptyAbove)
            return SurfaceType.Floor;

        if (hasTile && emptyBelow)
            return SurfaceType.Ceiling;

        if (hasTile && emptyLeft)
            return SurfaceType.LeftWall;

        if (hasTile && emptyRight)
            return SurfaceType.RightWall;

        return SurfaceType.None;
    }



    private bool TileExists(Vector3Int cell)
    {
        foreach (var tm in grid.GetComponentsInChildren<Tilemap>())
        {
            if (tm.GetTile(cell) != null)
                return true;
        }
        return false;
    }


    private void CancelPlacement()
    {
        if (currentGhost != null)
        {
            Destroy(currentGhost);
            currentGhost = null;
        }

        placingStickyWall = false;
        placingJumpPad = false;
        placingFan = false;
        placingPlug = false;
        isDragging = false;
        validPlacement = false;
    }

    public void StartPlacingStickyWall()
    {
        if (InventoryManager.Instance.GetCount(GadgetType.StickyWall) <= 0)
        {
            CancelPlacement();
            return;
        }

        CancelPlacement();
        placingStickyWall = true;
    }

    public void StartPlacingJumpPad()
    {
        if (InventoryManager.Instance.GetCount(GadgetType.JumpPad) <= 0)
        {
            CancelPlacement();
            return;
        }

        CancelPlacement();
        placingJumpPad = true;
    }

    public void StartPlacingFan()
    {
        if (InventoryManager.Instance.GetCount(GadgetType.Fan) <= 0)
        {
            CancelPlacement();
            return;
        }

        CancelPlacement();
        placingFan = true;
    }

    public void StartPlacingPlug()
    {
        if (InventoryManager.Instance.GetCount(GadgetType.Plug) <= 0)
        {
            CancelPlacement();
            return;
        }

        CancelPlacement();
        placingPlug = true;
    }
}
