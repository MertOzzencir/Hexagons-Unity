using System;
using System.Collections.Generic;
using System.Security;
using Mono.Cecil;
using NUnit.Framework.Internal;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class HexRenderer : MonoBehaviour
{
    private MeshRenderer mRenderer;
    private MeshFilter mFilter;
    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();
    private List<Color> vertexColors = new List<Color>();
    Mesh mesh;
    void Awake()
    {
        mRenderer = GetComponent<MeshRenderer>();
        mFilter = GetComponent<MeshFilter>();
    }

    public void SetVerticesAndTriangles(Vector3 center, int hexCount, float outerSize, float innerRadius, float height, Color vertexColor)
    {
        int currentHexIndex = 31 * hexCount;
        vertices.Add(new Vector3(center.x, center.y + height / 2, center.z));
        vertexColors.Add(vertexColor);
        for (int edgeIndex = 0; edgeIndex < 6; edgeIndex++)
        {
            FindPoints(innerRadius, outerSize, edgeIndex, height, center, vertexColor);
            FindPoints(innerRadius, outerSize, edgeIndex, center.y, center, vertexColor);
            InnerFindPoints(innerRadius, outerSize, edgeIndex, height / 2, center);
        }

        for (int faceCount = 1 + currentHexIndex; faceCount <= 26 + currentHexIndex; faceCount += 5)
        {
            if (faceCount == 26 + currentHexIndex)
            {
                #region Üst Yüzey
                triangles.Add(faceCount);
                triangles.Add(2 + currentHexIndex);
                triangles.Add(faceCount + 1);

                triangles.Add(faceCount);
                triangles.Add(1 + currentHexIndex);
                triangles.Add(2 + currentHexIndex);
                #endregion

                #region Alt Yüzey
                triangles.Add(faceCount + 2);
                triangles.Add(faceCount + 3);
                triangles.Add(4 + currentHexIndex);

                triangles.Add(faceCount + 2);
                triangles.Add(4 + currentHexIndex);
                triangles.Add(3 + currentHexIndex);
                #endregion
                #region Diş kenar
                triangles.Add(faceCount + 1);
                triangles.Add(faceCount - 22);
                triangles.Add(faceCount + 3);

                triangles.Add(faceCount + 1);
                triangles.Add(faceCount - 24);
                triangles.Add(faceCount - 22);
                #endregion
                #region İç kenar
                triangles.Add(faceCount);
                triangles.Add(3 + currentHexIndex);
                triangles.Add(1 + currentHexIndex);

                triangles.Add(faceCount);
                triangles.Add(faceCount + 2);
                triangles.Add(3 + currentHexIndex);
                #endregion
                #region İç Üçgenler
                triangles.Add(currentHexIndex);
                triangles.Add(currentHexIndex + 5);
                triangles.Add(faceCount + 4);
                #endregion
                break;
            }
            #region Üst Yüzey
            triangles.Add(faceCount);
            triangles.Add(faceCount + 6);
            triangles.Add(faceCount + 1);

            triangles.Add(faceCount);
            triangles.Add(faceCount + 5);
            triangles.Add(faceCount + 6);

            #endregion
            #region Alt Yüzey 
            triangles.Add(faceCount + 2);
            triangles.Add(faceCount + 3);
            triangles.Add(faceCount + 8);

            triangles.Add(faceCount + 2);
            triangles.Add(faceCount + 8);
            triangles.Add(faceCount + 7);

            #endregion
            #region Diş kenar
            triangles.Add(faceCount + 1);
            triangles.Add(faceCount + 6);
            triangles.Add(faceCount + 8);

            triangles.Add(faceCount + 1);
            triangles.Add(faceCount + 8);
            triangles.Add(faceCount + 3);
            #endregion
            #region İç kenar
            triangles.Add(faceCount);
            triangles.Add(faceCount + 7);
            triangles.Add(faceCount + 5);

            triangles.Add(faceCount);
            triangles.Add(faceCount + 2);
            triangles.Add(faceCount + 7);
            #endregion
            #region İç Üçgenler
            triangles.Add(currentHexIndex);
            triangles.Add(faceCount + 9);
            triangles.Add(faceCount + 4);
            #endregion
        }
    }

    private void FindPoints(float innerRadius, float outerRadius, int index, float height, Vector3 center, Color vertexColor)
    {
        vertices.Add(GetPoint(innerRadius, index, height, center));
        vertices.Add(GetPoint(outerRadius, index, height, center));
        vertexColors.Add(vertexColor);
        vertexColors.Add(vertexColor);

    }
    private void InnerFindPoints(float innerRadius, float outerRadius, int index, float height, Vector3 center)
    {
        vertices.Add(GetPoint(innerRadius, index, height, center));
        vertexColors.Add(Color.white);
    }

    private Vector3 GetPoint(float size, int index, float height, Vector3 center)
    {
        float angle_deg = 60 * index - 30;
        float angle_rad = Mathf.PI / 180f * angle_deg;
        return new Vector3(center.x + size * Mathf.Cos(angle_rad), center.y + height, center.z + size * Mathf.Sin(angle_rad));
    }
    public void ClearMeshData()
    {
        triangles.Clear();
        vertices.Clear();
        vertexColors.Clear();
    }
    public void GenerateMesh()
    {

        mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.colors = vertexColors.ToArray();
        mFilter.mesh = mesh;
        MeshCollider collider = GetComponent<MeshCollider>();
        if (collider == null)
        {
            MeshCollider sa = gameObject.AddComponent<MeshCollider>();
            sa.sharedMesh = mesh;
        }
        else
        {
            collider.sharedMesh = null;
            collider.sharedMesh = mesh;
        }

    }
}
