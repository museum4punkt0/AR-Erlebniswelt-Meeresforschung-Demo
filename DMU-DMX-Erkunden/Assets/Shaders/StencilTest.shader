Shader "Portal/StencilTest"
{
    Properties
    {
       [Enum(Equal,3,NotEqual,6)] stest("Stencil Test",int) = 3
    }
    SubShader
    {
	   Stencil {
		   Ref 1
		   comp[stest]
       }

        Pass
        {
            
        }
    }
}
