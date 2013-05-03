using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics;

namespace Common
{
    /// <summary>
    /// An interface that allows objects to be drawn.
    /// 
    /// An object that draws a Drawable object has the following pseudocode:
    /// 
    /// p = 0
    /// for i=0..NumDrawables-1
    ///     shape = DrawableType(i)
    ///     color = DrawableColor(i)
    ///     points = lookup(shape) //Constants.Graphics.DRAW_PT_LOOKUP_TABLE
    ///     for j=0..points-1
    ///         buffer[j] = GetPoint(p)
    ///         p = p + 1
    ///     end loop
    ///     draw shape with given color and point location buffer
    /// end loop
    /// 
    /// </summary>
    public interface Drawable
    {
        /// <summary>
        /// Returns the number of drawable components in this Drawable.
        /// </summary>
        int NumDrawables { get; }

        /// <summary>
        /// Returns the shape ID of the i_th component, where i = index. (Shape ID's are defined in Constants.Graphics) 
        /// </summary>
        /// <param name="index">The index of the component whose shape is to be returned.</param>
        /// <returns>The shape ID of the i_th component.</returns>
        int DrawableType(int index);
        /// <summary>
        /// Returns the color of the i_th component, where i = index.
        /// </summary>
        /// <param name="index">The index of the component whose color is to be returned.</param>
        /// <returns>The color of the i_th component.</returns>
        Color4 DrawableColor(int index);
        /// <summary>
        /// Returns the location of a point required to draw the i_th component. This index is cumulative, so it doesn't reset to zero after each component. (Number of points is defined in Constants.Graphics)
        /// </summary>
        /// <param name="index">The index of the location requested.</param>
        /// <returns>The location of a point required by the i_th component.</returns>
        Vector3 GetPoint(int index);
    }
}
