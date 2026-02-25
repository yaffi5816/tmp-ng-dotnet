# תכונת דילוג על Gemini API

## סקירה כללית
הוספנו אופציה לדלג על השליחה ל-API של Gemini ולקבל במקום זאת HTML דמה לצורכי בדיקה ופיתוח.

## איך להשתמש

### בצד הלקוח (Angular)
1. פתח את דף ה-Dashboard
2. סמן את התיבה "דלג על Gemini (השתמש ב-HTML דמה)"
3. העלה schema או הדבק אותו
4. לחץ על "Generate Dashboard"

### בצד השרת (API)
שלח בקשה POST ל-`/api/dashboard/generate` עם:

```json
{
  "schema": { ... },
  "components": ["BarChart", "KPI Card"],
  "skipGemini": true
}
```

## קבצים ששונו

### Backend (C#)
- `DTO/DashboardModels.cs` - הוספת שדה `SkipGemini`
- `Services/GeminiService.cs` - הוספת לוגיקה לדילוג ופונקציה `GenerateMockDashboard()`

### Frontend (Angular)
- `services/dashboard.service.ts` - הוספת פרמטר `skipGemini`
- `components/dashboard/dashboard.component.ts` - הוספת checkbox ולוגיקה

## יתרונות
- ✅ בדיקות מהירות ללא צריכת API quota
- ✅ פיתוח ללא תלות ב-API key
- ✅ תגובה מיידית
- ✅ חיסכון בעלויות

## HTML הדמה
ה-HTML הדמה כולל:
- 3 כרטיסי KPI
- גרף עמודות עם Chart.js
- עיצוב מודרני עם Tailwind CSS
- תמיכה בעברית (RTL)
