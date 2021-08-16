using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortBlock : MonoBehaviour
{
    public static SortBlock instance;
    GameManager gameManager;

    [SerializeField]
    public List<Block> sortRankBlock = new List<Block>();

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        sortRankBlock.Clear();

        SortBlocks();
    }

    public void SortBlockByQueue()
    {

    }

    public  int GetNumberInList(Block b)
    {
        if (sortRankBlock.Contains(b))
            return -(sortRankBlock.IndexOf(b) + 1);

        return -999;
    }

    public  void SortBlocks()
    {
        for (int i = 0; i < sortRankBlock.Count; i++)
        {
            sortRankBlock[i].GetComponent<SpriteRenderer>().sortingOrder = (sortRankBlock.IndexOf(sortRankBlock[i]) + 1);

            foreach (SpriteRenderer c in sortRankBlock[i].GetComponentsInChildren<SpriteRenderer>())
            {
                c.sortingOrder = (sortRankBlock.IndexOf(sortRankBlock[i]) + 1);
            }

            print(sortRankBlock[i]);
        }
    }

    public void SortOrderDuringDrag(Block b)
    {
        foreach (SpriteRenderer c in b.GetComponentsInChildren<SpriteRenderer>())
        {
            c.sortingOrder = b.GetComponent<SpriteRenderer>().sortingOrder;
        }
    }
}
