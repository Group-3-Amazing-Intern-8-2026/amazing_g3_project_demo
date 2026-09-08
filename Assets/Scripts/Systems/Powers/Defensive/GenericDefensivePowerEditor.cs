using UnityEngine;
using UnityEditor;
using Core.Enums;
using Systems.Powers.Defensive;

namespace Systems.Powers.Editor
{
    // Chỉ định rằng Editor này sẽ vẽ lại giao diện cho class GenericDefensivePower
    [CustomEditor(typeof(GenericDefensivePower))]
    [CanEditMultipleObjects]
    public class GenericDefensivePowerEditor : UnityEditor.Editor
    {
        SerializedProperty skillNameProp;
        SerializedProperty targetTypeProp;
        SerializedProperty targetSegmentProp;
        SerializedProperty bonusDefenseProp;
        SerializedProperty bonusMaxHealthProp;
        SerializedProperty defenseVFXPrefabProp;

        private void OnEnable()
        {
            // Liên kết các biến trong code với thuộc tính trên giao diện
            skillNameProp = serializedObject.FindProperty("skillName");
            targetTypeProp = serializedObject.FindProperty("targetType");
            targetSegmentProp = serializedObject.FindProperty("targetSegment");
            bonusDefenseProp = serializedObject.FindProperty("bonusDefense");
            bonusMaxHealthProp = serializedObject.FindProperty("bonusMaxHealth");
            defenseVFXPrefabProp = serializedObject.FindProperty("defenseVFXPrefab");
        }

        public override void OnInspectorGUI()
        {
            // Cập nhật dữ liệu mới nhất từ đối tượng
            serializedObject.Update();

            // 1. Vẽ vùng Skill Identity
            EditorGUILayout.LabelField("Skill Identity", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(skillNameProp);
            EditorGUILayout.Space(10);

            // 2. Vẽ vùng Targeting Settings
            EditorGUILayout.LabelField("Targeting Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(targetTypeProp);

            // LÓGIC THÔNG MINH: Chỉ vẽ ô Target Segment khi người dùng chọn SpecificSegment
            DefensiveTargetType currentTargetType = (DefensiveTargetType)targetTypeProp.enumValueIndex;
            if (currentTargetType == DefensiveTargetType.SpecificSegment)
            {
                EditorGUI.indentLevel++; // Thụt đầu dòng vào một chút nhìn cho đẹp
                EditorGUILayout.PropertyField(targetSegmentProp);
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space(10);

            // 3. Vẽ vùng Stats Modification
            EditorGUILayout.LabelField("Stats Modification", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(bonusDefenseProp);
            EditorGUILayout.PropertyField(bonusMaxHealthProp);
            EditorGUILayout.Space(10);

            // 4. Vẽ vùng Visual Effects
            EditorGUILayout.LabelField("Visual Effects (VFX)", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(defenseVFXPrefabProp);

            // Áp dụng các thay đổi cấu hình từ người dùng bấm trên Inspector vào code thực tế
            serializedObject.ApplyModifiedProperties();
        }
    }
}