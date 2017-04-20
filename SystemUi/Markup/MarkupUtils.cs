// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2017 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Globalization;
using LeopotamGroup.Common;
using LeopotamGroup.Math;
using LeopotamGroup.Serialization;
using LeopotamGroup.SystemUi.Actions;
using UnityEngine;
using UnityEngine.UI;

namespace LeopotamGroup.SystemUi.Markup {
    static class MarkupUtils {
        public static readonly int HashedOffset = "offset".GetStableHashCode ();

        public static readonly int HashedSize = "size".GetStableHashCode ();

        public static readonly int HashedDisabled = "disabled".GetStableHashCode ();

        static readonly int HashedColor = "color".GetStableHashCode ();

        public static readonly int HashedOnClick = "onClick".GetStableHashCode ();

        public static readonly int HashedOnBeginDrag = "onBeginDrag".GetStableHashCode ();

        public static readonly int HashedOnDrag = "onDrag".GetStableHashCode ();

        public static readonly int HashedOnEndDrag = "onEndDrag".GetStableHashCode ();

        public static readonly int HashedOnScroll = "onScroll".GetStableHashCode ();

        public static readonly int HashedOnDrop = "onDrop".GetStableHashCode ();

        public static readonly int HashedOnPressRelease = "onPressRelease".GetStableHashCode ();

        public static readonly int HashedOnEnterExit = "onEnterExit".GetStableHashCode ();

        public static readonly int HashedOnSelection = "onSelection".GetStableHashCode ();

        /// <summary>
        /// Process "disable"-attribute of node.
        /// </summary>
        /// <param name="node">Xml node.</param>
        /// <param name="container">markup container.</param>
        public static void SetDisabled (RectTransform rt, XmlNode node) {
            var attrValue = node.GetAttribute (HashedDisabled);
            if (string.CompareOrdinal (attrValue, "true") == 0) {
                rt.gameObject.SetActive (false);
            }
        }

        /// <summary>
        /// Process "color"-attribute of node.
        /// </summary>
        /// <param name="node">Xml node.</param>
        /// <param name="container">markup container.</param>
        public static void SetColor (Graphic widget, XmlNode node) {
            var attrValue = node.GetAttribute (HashedColor);
            if (!string.IsNullOrEmpty (attrValue)) {
                widget.color = attrValue.Length >= 8 ? attrValue.ToColor32 () : attrValue.ToColor24 ();
            }
        }

        /// <summary>
        /// Process interactive callbacks of node.
        /// </summary>
        /// <param name="node">Xml node.</param>
        /// <param name="container">markup container.</param>
        public static bool ValidateInteractive (RectTransform rt, XmlNode node) {
            var isInteractive = false;
            string attrValue;
            attrValue = node.GetAttribute (HashedOnClick);
            if (!string.IsNullOrEmpty (attrValue)) {
                rt.gameObject.AddComponent<UiClickAction> ().SetGroup (attrValue);
                isInteractive = true;
            }
            attrValue = node.GetAttribute (HashedOnDrag);
            if (!string.IsNullOrEmpty (attrValue)) {
                rt.gameObject.AddComponent<UiDragAction> ().SetGroup (attrValue);
                isInteractive = true;
            }
            attrValue = node.GetAttribute (HashedOnPressRelease);
            if (!string.IsNullOrEmpty (attrValue)) {
                rt.gameObject.AddComponent<UiPressReleaseAction> ().SetGroup (attrValue);
                isInteractive = true;
            }
            attrValue = node.GetAttribute (HashedOnScroll);
            if (!string.IsNullOrEmpty (attrValue)) {
                rt.gameObject.AddComponent<UiScrollAction> ().SetGroup (attrValue);
                isInteractive = true;
            }
            attrValue = node.GetAttribute (HashedOnDrop);
            if (!string.IsNullOrEmpty (attrValue)) {
                rt.gameObject.AddComponent<UiDropAction> ().SetGroup (attrValue);
                isInteractive = true;
            }
            attrValue = node.GetAttribute (HashedOnEnterExit);
            if (!string.IsNullOrEmpty (attrValue)) {
                rt.gameObject.AddComponent<UiEnterExitAction> ().SetGroup (attrValue);
                isInteractive = true;
            }
            attrValue = node.GetAttribute (HashedOnSelection);
            if (!string.IsNullOrEmpty (attrValue)) {
                rt.gameObject.AddComponent<UiSelectionAction> ().SetGroup (attrValue);
                isInteractive = true;
            }
            return isInteractive;
        }

        /// <summary>
        /// Process "offset" / "size" attributes of node.
        /// </summary>
        /// <param name="node">Xml node.</param>
        /// <param name="container">markup container.</param>
        public static void SetRectTransformSize (RectTransform rt, XmlNode node, bool ignoreSize = false) {
            var anchorMin = Vector2.zero;
            var anchorMax = Vector2.one;
            var offsetMin = Vector3.zero;
            var offsetMax = offsetMin;
            var point = Vector3.zero;
            string amountStr;
            float amount;
            string attrValue;

            if (!ignoreSize) {
                attrValue = node.GetAttribute (HashedSize);
                if (!string.IsNullOrEmpty (attrValue)) {
                    int percentIdx;
                    var parts = attrValue.Split (';');
                    if (parts.Length > 0 && !string.IsNullOrEmpty (parts[0])) {
                        amountStr = parts[0];
                        percentIdx = amountStr.IndexOf ('%');
                        if (percentIdx != -1) {
                            // relative.
                            if (float.TryParse (
                                    amountStr.Substring (0, percentIdx),
                                    NumberStyles.Float,
                                    MathExtensions.UnifiedNumberFormat,
                                    out amount)) {
                                amount *= 0.01f * 0.5f;
                                anchorMin.x = 0.5f - amount;
                                anchorMax.x = 0.5f + amount;
                            }
                        } else {
                            // absolute.
                            if (float.TryParse (amountStr, NumberStyles.Float, MathExtensions.UnifiedNumberFormat, out amount)) {
                                amount *= 0.5f;
                                anchorMin.x = 0.5f;
                                anchorMax.x = 0.5f;
                                offsetMin.x = -amount;
                                offsetMax.x = amount;
                            }
                        }
                    }
                    if (parts.Length > 1 && !string.IsNullOrEmpty (parts[1])) {
                        amountStr = parts[1];
                        percentIdx = amountStr.IndexOf ('%');
                        if (percentIdx != -1) {
                            // relative.
                            if (float.TryParse (
                                    amountStr.Substring (0, percentIdx),
                                    NumberStyles.Float,
                                    MathExtensions.UnifiedNumberFormat,
                                    out amount)) {
                                amount *= 0.01f * 0.5f;
                                anchorMin.y = 0.5f - amount;
                                anchorMax.y = 0.5f + amount;
                            }
                        } else {
                            // absolute.
                            if (float.TryParse (amountStr, NumberStyles.Float, MathExtensions.UnifiedNumberFormat, out amount)) {
                                amount *= 0.5f;
                                anchorMin.y = 0.5f;
                                anchorMax.y = 0.5f;
                                offsetMin.y = -amount;
                                offsetMax.y = amount;
                            }
                        }
                    }
                }
            }

            attrValue = node.GetAttribute (HashedOffset);
            if (!string.IsNullOrEmpty (attrValue)) {
                var parts = attrValue.Split (';');
                if (parts.Length > 0 && !string.IsNullOrEmpty (parts[0])) {
                    if (float.TryParse (parts[0], NumberStyles.Float, MathExtensions.UnifiedNumberFormat, out amount)) {
                        point.x = amount;
                    }
                }
                if (parts.Length > 1 && !string.IsNullOrEmpty (parts[1])) {
                    if (float.TryParse (parts[1], NumberStyles.Float, MathExtensions.UnifiedNumberFormat, out amount)) {
                        point.y = amount;
                    }
                }
            }

            rt.anchorMin = anchorMin;
            rt.anchorMax = anchorMax;
            rt.offsetMin = offsetMin;
            rt.offsetMax = offsetMax;
            rt.localPosition = point;
        }
    }
}