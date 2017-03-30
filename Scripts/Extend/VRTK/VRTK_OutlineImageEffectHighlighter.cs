﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HighlightingSystem;

namespace VRTK.Highlighters
{
    public class VRTK_OutlineImageEffectHighlighter : VRTK_BaseHighlighter
    {
        public Color OutLineColor;

        private Highlighter _highLighter;

        public override void Initialise(Color? color = default(Color?), Dictionary<string, object> options = null)
        {
            usesClonedObject = false;

            _highLighter = GetComponent<Highlighter>();
            if (_highLighter == null)
            {
                _highLighter = gameObject.AddComponent<Highlighter>();
            }
            //SetOptions(options);
            ResetHighlighter();
        }

        public override void Highlight(Color? color = default(Color?), float duration = 0)
        {
            if (color == null)
            {
                return;
            }
            else
            {
                if (OutLineColor == Color.clear)
                {
                    _highLighter.ConstantOnImmediate((Color)color);
                }
                else
                {
                    _highLighter.ConstantOnImmediate(OutLineColor);
                }
            }

        }

        private void SetOptions(Dictionary<string, object> options = null)
        {
            Color defaultColor = GetOption<Color>(options, "OutLineColor");

            if (defaultColor != Color.clear)
            {
                OutLineColor = defaultColor;
            }

        }

        public override void Unhighlight(Color? color = default(Color?), float duration = 0)
        {
            _highLighter.Off();
        }

        public override void ResetHighlighter()
        {
            _highLighter.Off();
            _highLighter.ReinitMaterials();
        }


    }
}
