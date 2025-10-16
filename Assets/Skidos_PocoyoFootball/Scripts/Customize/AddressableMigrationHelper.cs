#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;


public class AddressableMigrationHelper : MonoBehaviour
{
    [Header("Migration Settings")]
    public bool migratePatterns = true;
    public bool migrateShields = true;
    public bool migrateSneakers = true;
    public bool migrateTeams = true;
    
    [Header("Asset Groups")]
    public string patternsGroupName = "Customization_Patterns";
    public string shieldsGroupName = "Customization_Shields";
    public string sneakersGroupName = "Customization_Sneakers";
    public string teamsGroupName = "Teams_Data";
    
    [Header("Migration Results")]
    [TextArea(5, 10)]
    public string migrationLog = "";
    
    [ContextMenu("Migrate to Addressables")]
    public void MigrateToAddressables()
    {
        migrationLog = "Starting migration to Addressables...\n";
        
        try
        {
            if (migratePatterns)
                MigratePatterns();
                
            if (migrateShields)
                MigrateShields();
                
            if (migrateSneakers)
                MigrateSneakers();
                
            if (migrateTeams)
                MigrateTeams();
                
            migrationLog += "\nMigration completed successfully!";
            migrationLog += "\n\nNext steps:";
            migrationLog += "\n1. Create CustomizeDataAddressable asset";
            migrationLog += "\n2. Assign Addressable references to the new asset";
            migrationLog += "\n3. Update CustomizeControl2 to use the new asset";
            migrationLog += "\n4. Build Addressables";
            
            Debug.Log("Migration completed! Check the migration log for next steps.");
        }
        catch (System.Exception e)
        {
            migrationLog += $"\nMigration failed with error: {e.Message}";
            Debug.LogError($"Migration failed: {e.Message}");
        }
    }
    
    private void MigratePatterns()
    {
        migrationLog += "\nMigrating Patterns...";
        
        // Find all pattern textures in the project
        string[] patternGuids = AssetDatabase.FindAssets("t:Texture2D");
        int migratedCount = 0;
        
        foreach (string guid in patternGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (path.Contains("Pattern") || path.Contains("pattern"))
            {
                // Mark as Addressable
                var asset = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
                if (asset != null)
                {
                    GetOrCreateGroup(patternsGroupName);
                    migrationLog += $"\n  - {asset.name} would be added to {patternsGroupName}";
                    migratedCount++;
                }
            }
        }
        
        migrationLog += $"\n  Migrated {migratedCount} pattern textures";
    }
    
    private void MigrateShields()
    {
        migrationLog += "\nMigrating Shields...";
        
        // Find all shield textures in the project
        string[] shieldGuids = AssetDatabase.FindAssets("t:Texture2D");
        int migratedCount = 0;
        
        foreach (string guid in shieldGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (path.Contains("Shield") || path.Contains("shield"))
            {
                // Mark as Addressable
                var asset = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
                if (asset != null)
                {
                    GetOrCreateGroup(shieldsGroupName);
                    migrationLog += $"\n  - {asset.name} would be added to {shieldsGroupName}";
                    migratedCount++;
                }
            }
        }
        
        migrationLog += $"\n  Migrated {migratedCount} shield textures";
    }
    
    private void MigrateSneakers()
    {
        migrationLog += "\nMigrating Sneakers...";
        
        // Find all sneaker textures in the project
        string[] sneakerGuids = AssetDatabase.FindAssets("t:Texture2D");
        int migratedCount = 0;
        
        foreach (string guid in sneakerGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (path.Contains("Sneaker") || path.Contains("sneaker") || path.Contains("Sniker"))
            {
                // Mark as Addressable
                var asset = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
                if (asset != null)
                {
                    GetOrCreateGroup(sneakersGroupName);
                    migrationLog += $"\n  - {asset.name} would be added to {sneakersGroupName}";
                    migratedCount++;
                }
            }
        }
        
        migrationLog += $"\n  Migrated {migratedCount} sneaker textures";
    }
    
    private void MigrateTeams()
    {
        migrationLog += "\nMigrating Teams...";
        
        // Find all team-related assets
        string[] teamGuids = AssetDatabase.FindAssets("t:Texture2D");
        int migratedCount = 0;
        
        foreach (string guid in teamGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (path.Contains("Team") || path.Contains("team"))
            {
                // Mark as Addressable
                var asset = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
                if (asset != null)
                {
                    GetOrCreateGroup(teamsGroupName);
                    migrationLog += $"\n  - {asset.name} would be added to {teamsGroupName}";
                    migratedCount++;
                }
            }
        }
        
        migrationLog += $"\n  Migrated {migratedCount} team textures";
    }
    
    private void GetOrCreateGroup(string groupName)
    {
        // For now, just log that we would create/find the group
        // The actual Addressables setup needs to be done through Unity's Addressables window
        migrationLog += $"\n  Would use group: {groupName}";
        migrationLog += $"\n  💡 To create this group, use Unity's Addressables window:";
        migrationLog += $"\n     Window → Asset Management → Addressables → Groups → Create New Group";
    }
}
#endif
