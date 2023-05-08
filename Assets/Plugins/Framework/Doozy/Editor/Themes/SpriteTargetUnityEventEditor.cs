// Copyright (c) 2015 - 2020 Doozy Entertainment. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using Doozy.Editor.Internal;
using Doozy.Engine.Themes;
using Doozy.Engine.Utils;
using System;
using UnityEditor;
using UnityEngine;

namespace Doozy.Editor.Themes
{
    [CustomEditor(typeof(SpriteTargetUnityEvent))]
    [CanEditMultipleObjects]
    public class SpriteTargetUnityEventEditor : BaseEditor
    {
        protected override ColorName ComponentColorName
        {
            get { return DGUI.Colors.ThemesColorName; }
        }

        private SpriteTargetUnityEvent m_target;

        private SpriteTargetUnityEvent Target
        {
            get
            {
                if (m_target != null) return m_target;
                m_target = (SpriteTargetUnityEvent)target;
                return m_target;
            }
        }

        private static ThemesDatabase Database
        {
            get { return ThemesSettings.Database; }
        }

        private string[] ThemesNames;
        private string[] VariantsNames;

        private SerializedProperty m_event;

        private ThemeData m_theme;
        private LabelId m_Property;
        private int m_selectedThemeIndex;
        private int m_selectedVariantIndex;
        private int m_selectedPropertyIndex;

        private bool HasReference
        {
            get { return m_event.objectReferenceValue != null; }
        }

        protected override void LoadSerializedProperty()
        {
            base.LoadSerializedProperty();

            m_event = GetProperty(PropertyName.Event);
        }

        public override void OnInspectorGUI()
        {
            UpdateIds();
            UpdateLists();
            base.OnInspectorGUI();
            serializedObject.Update();
            DrawHeader(Styles.GetStyle(Styles.StyleName.ComponentHeaderSpriteTargetUnityEvent), MenuUtils.SpriteTargetUnityEvent_Manual, MenuUtils.SpriteTargetUnityEvent_YouTube);
            GUILayout.Space(DGUI.Properties.Space(2));
            DGUI.Property.UnityEvent(m_event, UILabels.Event, ComponentColorName, Target.Event.GetPersistentEventCount());
            GUILayout.Space(DGUI.Properties.Space(4));
            int themeIndex = Database.GetThemeIndex(Target.ThemeId);
            if (themeIndex != -1)
            {
                ThemeTargetEditorUtils.DrawThemePopup(Database, m_theme, ThemesNames, themeIndex, ComponentColorName, serializedObject, targets, Target, InitialGUIColor, UpdateIds, UpdateLists);
                GUILayout.Space(DGUI.Properties.Space());
                ThemeTargetEditorUtils.DrawActiveVariant(m_theme, ComponentColorName);
            }

            GUILayout.Space(DGUI.Properties.Space(2));
            int propertyIndex = m_theme.GetSpritePropertyIndex(Target.PropertyId);
            if (Target.PropertyId == Guid.Empty || propertyIndex == -1) ThemeTargetEditorUtils.DrawLabelNoPropertyFound();
            else ThemeTargetEditorUtils.DrawSpriteProperties(m_theme, propertyIndex, serializedObject, targets, Target, ComponentColorName, InitialGUIColor);
            GUILayout.Space(DGUI.Properties.Space(4));
            serializedObject.ApplyModifiedProperties();
        }

        private void UpdateIds()
        {
            if (!Database.Contains(Target.ThemeId))
                Target.ThemeId = Database.Themes[0].Id;
            m_theme = Database.GetThemeData(Target.ThemeId);

            if (!m_theme.ContainsSpriteProperty(Target.PropertyId))
                Target.PropertyId = m_theme.SpriteLabels.Count > 0
                    ? m_theme.SpriteLabels[0].Id
                    : Guid.Empty;
        }


        private void UpdateLists()
        {
            ThemesNames = ThemesDatabase.GetThemesNames(Database);
        }
    }
}