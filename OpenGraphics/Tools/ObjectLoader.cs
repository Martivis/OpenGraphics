
using OpenTK.Graphics.OpenGL4;
using OpenTK.Platform.Windows;

namespace OpenGraphics;

public static class ObjectLoader
{
    static float[] _vertices1 = {
        // Position               Normals                Texture 
        -0.5f, -0.5f,  0.5f,    0.0f,  0.0f,  1.0f,    0.0f, 0.0f, // Front face
         0.5f, -0.5f,  0.5f,    0.0f,  0.0f,  1.0f,    1.0f, 0.0f,
         0.5f,  0.5f,  0.5f,    0.0f,  0.0f,  1.0f,    1.0f, 1.0f,
        -0.5f,  0.5f,  0.5f,    0.0f,  0.0f,  1.0f,    0.0f, 1.0f,
                                                       
        // Position               Normals                Texture                               
        -0.5f, -0.5f, -0.5f,    0.0f,  0.0f, -1.0f,    1.0f, 0.0f, // Back face   
        -0.5f,  0.5f, -0.5f,    0.0f,  0.0f, -1.0f,    1.0f, 1.0f,
         0.5f,  0.5f, -0.5f,    0.0f,  0.0f, -1.0f,    0.0f, 1.0f,
         0.5f, -0.5f, -0.5f,    0.0f,  0.0f, -1.0f,    0.0f, 0.0f,
                                                       
        // Position               Normals                Texture                                
        -0.5f,  0.5f, -0.5f,    0.0f,  1.0f,  0.0f,    0.0f, 1.0f, // Top face   
        -0.5f,  0.5f,  0.5f,    0.0f,  1.0f,  0.0f,    0.0f, 0.0f,
         0.5f,  0.5f,  0.5f,    0.0f,  1.0f,  0.0f,    1.0f, 0.0f,
         0.5f,  0.5f, -0.5f,    0.0f,  1.0f,  0.0f,    1.0f, 1.0f,
                                                       
        // Position               Normals                Texture                              
        -0.5f, -0.5f, -0.5f,    0.0f, -1.0f,  0.0f,    0.0f, 0.0f, // Bottom face  
         0.5f, -0.5f, -0.5f,    0.0f, -1.0f,  0.0f,    1.0f, 0.0f,
         0.5f, -0.5f,  0.5f,    0.0f, -1.0f,  0.0f,    1.0f, 1.0f,
        -0.5f, -0.5f,  0.5f,    0.0f, -1.0f,  0.0f,    0.0f, 1.0f,
                                                       
        // Position               Normals                Texture                              
         0.5f, -0.5f, -0.5f,    1.0f,  0.0f,  0.0f,    1.0f, 0.0f, // Right face    
         0.5f,  0.5f, -0.5f,    1.0f,  0.0f,  0.0f,    1.0f, 1.0f,
         0.5f,  0.5f,  0.5f,    1.0f,  0.0f,  0.0f,    0.0f, 1.0f,
         0.5f, -0.5f,  0.5f,    1.0f,  0.0f,  0.0f,    0.0f, 0.0f,
                                                       
        // Position               Normals                Texture                            
        -0.5f, -0.5f, -0.5f,   -1.0f,  0.0f,  0.0f,    0.0f, 0.0f, // Left face       
        -0.5f, -0.5f,  0.5f,   -1.0f,  0.0f,  0.0f,    1.0f, 0.0f,
        -0.5f,  0.5f,  0.5f,   -1.0f,  0.0f,  0.0f,    1.0f, 1.0f,
        -0.5f,  0.5f, -0.5f,   -1.0f,  0.0f,  0.0f,    0.0f, 1.0f
    };

    static uint[] _indices1 = {
        0, 1, 2, 0, 2, 3,       // Front face
        4, 5, 6, 4, 6, 7,       // Back face
        8, 9, 10, 8, 10, 11,    // Top face
        12, 13, 14, 12, 14, 15, // Bottom face
        16, 17, 18, 16, 18, 19, // Right face
        20, 21, 22, 20, 22, 23  // Left face
    };

    static Dictionary<string, VBODataFormat> _format;

    static ObjectLoader()
    {
        _format = new Dictionary<string, VBODataFormat>
        {
            {
                "aPosition",
                new VBODataFormat()
                {
                    Type = VertexAttribPointerType.Float,
                    Dimention = 3,
                    Normalized = false,
                    Stride = 8 * sizeof(float),
                    Offset = 0
                }
            },
            {
                "aNormal",
                new VBODataFormat()
                {
                    Type = VertexAttribPointerType.Float,
                    Dimention = 3,
                    Normalized = false,
                    Stride = 8 * sizeof(float),
                    Offset = 3 * sizeof(float),
                }
            },
            {
                "aTexCoord",
                new VBODataFormat()
                {
                    Type = VertexAttribPointerType.Float,
                    Dimention = 2,
                    Normalized = false,
                    Stride = 8 * sizeof(float),
                    Offset = 6 * sizeof(float),
                }
            }
        };
    }

    public static VertexData GetObject(string filename)
    {
        return new VertexData(_vertices1, _indices1, _format);
    }
}
