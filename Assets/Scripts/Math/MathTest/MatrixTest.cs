using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixTest : MonoBehaviour {
	void Start () {
        Matrix a = new Matrix("[[1,2,2,3,2],[2,1,3,5,1],[3,3,4,1,5],[4,4,5,1,3],[4,6,5,2,3]]");
        Matrix b = Matrix.Reverse(a);
        Debug.Log(b.ToString());
        Debug.Log((a * b).ToString());
    }
}