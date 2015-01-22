using UnityEngine;
using System.Collections;

public class ItemStatus : MonoBehaviour 
{
	public int scanned = 0;
	public GameObject inTrigger = null;
	public Customer customer = null;
	public string name;
	public bool scannedInOvertime = false;
	public bool autoDragged = false;
}
