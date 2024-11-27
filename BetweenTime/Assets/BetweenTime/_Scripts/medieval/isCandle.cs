using UnityEngine;

public class isCandle : MonoBehaviour
{
    public candleManager cM;
    public int candleNr = 0;

    public void Start()
    {
        cM.listCandle(GetComponent<isCandle>());
    }

    public void placedOnSocket()
    {
        cM.candlePlacedOnSocket(candleNr);
    }

}
