using System;
using System.Data;
using System.Linq;
using System.Text;
using TK_RTMS.Manager;

namespace TK_RTMS.CommonCS
{
    class DB_Process
    {
        #region 변수

        private static DB_Process instance = null;

        #endregion

        /// <summary>
        /// DB_Process 싱글톤 패턴 객체 생성
        /// </summary>
        public static DB_Process Instance
        {
            get
            {
                if (instance == null)
                    instance = new DB_Process();

                return instance;
            }
        }

        /// <summary>
        /// Pareto Chart Main Data 조회
        /// </summary>
        /// <param name="opt"></param>
        /// <returns></returns>
        public DataSet Get_ParetoChartMainData(string RItem, SearchOption opt)
        {
            string query = @"
WITH
TOTAL_INSP_CNT_TBL AS
(
	SELECT COUNT(*) AS TOTAL_CNT
	FROM [TKEK_PRD].[dbo].TB_JOB_WORK_PP_SPC A
    INNER JOIN [TKEK_PRD].[dbo].TB_JOB_WORK_PP B
	ON A.PLAN_SEQ = B.PLAN_SEQ AND A.PLAN_DT = B.PLAN_DT
	WHERE 1 = 1
    AND A.PLAN_DT BETWEEN '#START_TIME' AND '#END_TIME'
    --JUDGE #JUDGE_CONDITION
    --LR #LR_CONDITION
    --TYPE #TYPE_CONDITION
    --MODEL #MODEL_CONDITION
    --OPENING #OPENING_CONDITION
    --MATERIALS #MATERIALS_CONDITION
    --EQPID #EQPID_CONDITION
),
MATERIALS_CNT_TBL AS
(
   SELECT SUBSTRING(TBL.Materials, CHARINDEX(' ', TBL.Materials), LEN(TBL.Materials))  AS MATERIALS, COUNT(*) AS CNT
   FROM 
   (
	SELECT SUBSTRING(A.MTR_NM, CHARINDEX('CO', A.MTR_NM), LEN(A.MTR_NM)) AS Materials
	FROM [TKEK_PRD].[dbo].TB_JOB_WORK_PP_SPC A
    INNER JOIN [TKEK_PRD].[dbo].TB_JOB_WORK_PP B
	ON A.PLAN_SEQ = B.PLAN_SEQ AND A.PLAN_DT = B.PLAN_DT
	WHERE 1 = 1
    AND A.PLAN_DT BETWEEN '#START_TIME' AND '#END_TIME'
    --JUDGE #JUDGE_CONDITION
    --LR #LR_CONDITION
    --TYPE #TYPE_CONDITION
    --MODEL #MODEL_CONDITION
    --OPENING #OPENING_CONDITION
    --MATERIALS #MATERIALS_CONDITION
    --EQPID #EQPID_CONDITION
   ) TBL
   WHERE 1 = 1
   GROUP BY SUBSTRING(TBL.Materials, CHARINDEX(' ', TBL.Materials), LEN(TBL.Materials))
)
SELECT TOP #NUMBER A.MATERIALS, A.CNT, B.TOTAL_CNT, ROUND(CONVERT(float, A.CNT) / B.TOTAL_CNT, 4) * 100 AS '점유율'
FROM MATERIALS_CNT_TBL A
LEFT JOIN TOTAL_INSP_CNT_TBL B
ON 1 = 1
WHERE 1 =1
ORDER BY 4 DESC

";
            query = query.Replace("#NUMBER", opt.TOPN);
            query = GetCommonConditionQuery(query, opt);
            query = CreateColumns(query, RItem);

            DataSet ds = new DataSet();
            SqlDBManager.Instance.ExecuteDsQuery(ds, query);

            return ds;
        }

        /// <summary>
        /// Pareto Chart Sub Data 조회
        /// </summary>
        /// <param name="cType"></param>
        /// <param name="materials"></param>
        /// <returns></returns>
        public DataSet Get_ParetoChart_SubData(string RItem, SearchOption opt, string materials)
        {
            string query = @"

WITH
TOTAL_INSP_CNT_TBL AS
(
	SELECT COUNT(*) AS TOTAL_CNT
	FROM [TKEK_PRD].[dbo].TB_JOB_WORK_PP_SPC A
    INNER JOIN [TKEK_PRD].[dbo].TB_JOB_WORK_PP B
	ON A.PLAN_SEQ = B.PLAN_SEQ AND A.PLAN_DT = B.PLAN_DT
	WHERE 1 = 1
    AND A.PLAN_DT BETWEEN '#START_TIME' AND '#END_TIME'
    --JUDGE #JUDGE_CONDITION
    --LR #LR_CONDITION
    --TYPE #TYPE_CONDITION
    --MODEL #MODEL_CONDITION
    --OPENING #OPENING_CONDITION
    --SUB_MATERIALS #SUB_MATERIALS_CONDITION
    --EQPID #EQPID_CONDITION
),
OPENING_CNT_TBL AS
(
   SELECT Opening, COUNT(*) AS CNT
   FROM 
   (
	SELECT SUBSTRING(A.Materials, 0, CHARINDEX('*', A.Materials)) AS Opening
	FROM
	(
		SELECT SUBSTRING(A.MTR_NM, CHARINDEX('CO', A.MTR_NM), LEN(A.MTR_NM)) AS Materials
		FROM [TKEK_PRD].[dbo].TB_JOB_WORK_PP_SPC A
		INNER JOIN [TKEK_PRD].[dbo].TB_JOB_WORK_PP B
		ON A.PLAN_SEQ = B.PLAN_SEQ AND A.PLAN_DT = B.PLAN_DT
		WHERE 1 = 1
    AND A.PLAN_DT BETWEEN '#START_TIME' AND '#END_TIME'
    --JUDGE #JUDGE_CONDITION
    --LR #LR_CONDITION
    --TYPE #TYPE_CONDITION
    --MODEL #MODEL_CONDITION
    --OPENING #OPENING_CONDITION
    --SUB_MATERIALS #SUB_MATERIALS_CONDITION
    --EQPID #EQPID_CONDITION
    ) A
   ) TBL
   WHERE 1 = 1
   GROUP BY Opening
)
SELECT TOP #NUMBER A.Opening, A.CNT, B.TOTAL_CNT, ROUND(CONVERT(float, A.CNT) / B.TOTAL_CNT, 4) * 100 AS '점유율'
FROM OPENING_CNT_TBL A
LEFT JOIN TOTAL_INSP_CNT_TBL B
ON 1 = 1
WHERE 1 =1
ORDER BY 4 DESC

";
            query = query.Replace("--SUB_MATERIALS", "");
            query = query.Replace("#SUB_MATERIALS_CONDITION", $"AND CHARINDEX('{materials}', A.MTR_NM) > 0 ");

            query = query.Replace("#NUMBER", opt.TOPN);
            query = GetCommonConditionQuery(query, opt);
            query = CreateColumns(query, RItem);

            DataSet ds = new DataSet();
            SqlDBManager.Instance.ExecuteDsQuery(ds, query);

            return ds;
        }

        /// <summary>
        /// Pareto Chart Sub Data 조회
        /// </summary>
        /// <param name="cType"></param>
        /// <param name="materials"></param>
        /// <returns></returns>
        public DataSet Get_ParetoChart_OpeningData(string RItem, SearchOption opt, string materials, string opening)
        {
            string query = @"

WITH
TOTAL_INSP_CNT_TBL AS
(
	SELECT COUNT(*) AS TOTAL_CNT
	FROM [TKEK_PRD].[dbo].TB_JOB_WORK_PP_SPC A
    INNER JOIN [TKEK_PRD].[dbo].TB_JOB_WORK_PP B
	ON A.PLAN_SEQ = B.PLAN_SEQ AND A.PLAN_DT = B.PLAN_DT
	WHERE 1 = 1
    AND A.PLAN_DT BETWEEN '#START_TIME' AND '#END_TIME'
    --JUDGE #JUDGE_CONDITION
    --LR #LR_CONDITION
    --TYPE #TYPE_CONDITION
    --MODEL #MODEL_CONDITION
    --OPENING #OPENING_CONDITION
    --SUB_OPENING #SUB_OPENING_CONDITION
    --SUB_MATERIALS #SUB_MATERIALS_CONDITION
    --EQPID #EQPID_CONDITION
),
OPENING_CNT_TBL AS
(
   SELECT Opening, COUNT(*) AS CNT
   FROM 
   (
	SELECT SUBSTRING(A.Materials, 0, CHARINDEX('*', A.Materials)) AS Opening
	FROM
	(
		SELECT SUBSTRING(A.MTR_NM, CHARINDEX('CO', A.MTR_NM), LEN(A.MTR_NM)) AS Materials
		FROM [TKEK_PRD].[dbo].TB_JOB_WORK_PP_SPC A
		INNER JOIN [TKEK_PRD].[dbo].TB_JOB_WORK_PP B
		ON A.PLAN_SEQ = B.PLAN_SEQ AND A.PLAN_DT = B.PLAN_DT
		WHERE 1 = 1
    AND A.PLAN_DT BETWEEN '#START_TIME' AND '#END_TIME'
    --JUDGE #JUDGE_CONDITION
    --LR #LR_CONDITION
    --TYPE #TYPE_CONDITION
    --MODEL #MODEL_CONDITION
    --OPENING #OPENING_CONDITION
    --SUB_OPENING #SUB_OPENING_CONDITION
    --SUB_MATERIALS #SUB_MATERIALS_CONDITION
    --EQPID #EQPID_CONDITION
    ) A
   ) TBL
   WHERE 1 = 1
   GROUP BY Opening
)
SELECT TOP #NUMBER A.Opening, A.CNT, B.TOTAL_CNT, ROUND(CONVERT(float, A.CNT) / B.TOTAL_CNT, 4) * 100 AS '점유율'
FROM OPENING_CNT_TBL A
LEFT JOIN TOTAL_INSP_CNT_TBL B
ON 1 = 1
WHERE 1 =1
ORDER BY 4 DESC

";

            query = query.Replace("--SUB_OPENING", "");
            query = query.Replace("#SUB_OPENING_CONDITION", $"AND CHARINDEX('{opening}', A.MTR_NM) > 0 ");

            query = query.Replace("--SUB_MATERIALS", "");
            query = query.Replace("#SUB_MATERIALS_CONDITION", $"AND CHARINDEX('{materials}', A.MTR_NM) > 0 ");

            query = query.Replace("#NUMBER", opt.TOPN);
            query = GetCommonConditionQuery(query, opt);
            query = CreateColumns(query, RItem);

            DataSet ds = new DataSet();
            SqlDBManager.Instance.ExecuteDsQuery(ds, query);

            return ds;
        }

        /// <summary>
        /// Histogram Chart Main Data 조회
        /// </summary>
        /// <param name="opt"></param>
        /// <returns></returns>
        public DataSet Get_HistogramMainData(string RItem, SearchOption opt)
        {
            string query = @"
WITH
RAW_TBL AS
(
	SELECT A.PLAN_DT, A.REG_DT, A.JOB_NO, A.JOB_HG, A.MTR_NM, A.DWG_NO,
		   #BASE AS tVal, 
		   #TOLERANCE AS inRange,
		   #RESULT_VALUE AS R_#ITEM,
		   #RESULT_JUDGE AS JUDGE
	FROM [TKEK_PRD].[dbo].TB_JOB_WORK_PP_SPC A
    INNER JOIN [TKEK_PRD].[dbo].TB_JOB_WORK_PP B
	ON A.PLAN_SEQ = B.PLAN_SEQ AND A.PLAN_DT = B.PLAN_DT
	WHERE 1 = 1
    AND A.PLAN_DT BETWEEN '#START_TIME' AND '#END_TIME'
    --JUDGE #JUDGE_CONDITION
    --LR #LR_CONDITION
    --TYPE #TYPE_CONDITION
    --MODEL #MODEL_CONDITION
    --OPENING #OPENING_CONDITION
    --MATERIALS #MATERIALS_CONDITION
    --EQPID #EQPID_CONDITION
)
SELECT PLAN_DT, REG_DT, JOB_NO, JOB_HG, MTR_NM,
       ROUND(R_#ITEM, 2) AS R_#ITEM,
	   ROUND(R_#ITEM - tVal , 2) AS GAP, 
	   inRange
FROM RAW_TBL
WHERE 1 = 1
";
            query = GetCommonConditionQuery(query, opt);
            query = CreateColumns(query, RItem);

            DataSet ds = new DataSet();
            SqlDBManager.Instance.ExecuteDsQuery(ds, query);

            return ds;
        }

        /// <summary>
        /// Histogram Chart Sub Data 조회
        /// </summary>
        /// <param name="opt"></param>
        /// <returns></returns>
        public DataSet Get_HistogramSubData(string RItem, SearchOption opt, string materials)
        {
            string query = @"
WITH
RAW_TBL AS
(
	SELECT A.PLAN_DT, A.REG_DT, A.JOB_NO, A.JOB_HG, A.MTR_NM, A.DWG_NO,
		   #BASE AS tVal, 
		   #TOLERANCE AS inRange,
		   #RESULT_VALUE AS R_#ITEM,
		   #RESULT_JUDGE AS JUDGE
	FROM [TKEK_PRD].[dbo].TB_JOB_WORK_PP_SPC A
    INNER JOIN [TKEK_PRD].[dbo].TB_JOB_WORK_PP B
	ON A.PLAN_SEQ = B.PLAN_SEQ AND A.PLAN_DT = B.PLAN_DT
	WHERE 1 = 1
    AND A.PLAN_DT BETWEEN '#START_TIME' AND '#END_TIME'
    --JUDGE #JUDGE_CONDITION
    --LR #LR_CONDITION
    --TYPE #TYPE_CONDITION
    --MODEL #MODEL_CONDITION
    --OPENING #OPENING_CONDITION
    --SUB_MATERIALS #SUB_MATERIALS_CONDITION
    --EQPID #EQPID_CONDITION
)
SELECT PLAN_DT, REG_DT, JOB_NO, JOB_HG, MTR_NM,
       ROUND(R_#ITEM, 2) AS R_#ITEM,
	   ROUND(R_#ITEM - tVal , 2) AS GAP, 
	   inRange
FROM RAW_TBL
WHERE 1 = 1
";
            query = query.Replace("--SUB_MATERIALS", "");
            query = query.Replace("#SUB_MATERIALS_CONDITION", $"AND CHARINDEX('{materials}', A.MTR_NM) > 0 ");

            query = GetCommonConditionQuery(query, opt);
            query = CreateColumns(query, RItem);

            DataSet ds = new DataSet();
            SqlDBManager.Instance.ExecuteDsQuery(ds, query);

            return ds;
        }

        /// <summary>
        /// Histogram Chart Sub Data 조회
        /// </summary>
        /// <param name="opt"></param>
        /// <returns></returns>
        public DataSet Get_HistogramOpeningData(string RItem, SearchOption opt, string opening, string materials)
        {
            string query = @"
WITH
RAW_TBL AS
(
	SELECT A.PLAN_DT, A.REG_DT, A.JOB_NO, A.JOB_HG, A.MTR_NM, A.DWG_NO,
		   #BASE AS tVal, 
		   #TOLERANCE AS inRange,
		   #RESULT_VALUE AS R_#ITEM,
		   #RESULT_JUDGE AS JUDGE
	FROM [TKEK_PRD].[dbo].TB_JOB_WORK_PP_SPC A
    INNER JOIN [TKEK_PRD].[dbo].TB_JOB_WORK_PP B
	ON A.PLAN_SEQ = B.PLAN_SEQ AND A.PLAN_DT = B.PLAN_DT
	WHERE 1 = 1
    AND A.PLAN_DT BETWEEN '#START_TIME' AND '#END_TIME'
    --JUDGE #JUDGE_CONDITION
    --LR #LR_CONDITION
    --TYPE #TYPE_CONDITION
    --MODEL #MODEL_CONDITION
    --OPENING #OPENING_CONDITION
    --SUB_OPENING #SUB_OPENING_CONDITION
    --SUB_MATERIALS #SUB_MATERIALS_CONDITION
    --EQPID #EQPID_CONDITION
)
SELECT PLAN_DT, REG_DT, JOB_NO, JOB_HG, MTR_NM,
       ROUND(R_#ITEM, 2) AS R_#ITEM,
	   ROUND(R_#ITEM - tVal , 2) AS GAP, 
	   inRange
FROM RAW_TBL
WHERE 1 = 1
";
            query = query.Replace("--SUB_OPENING", "");
            query = query.Replace("#SUB_OPENING_CONDITION", $"AND CHARINDEX('{opening}', A.MTR_NM) > 0 ");

            query = query.Replace("--SUB_MATERIALS", "");
            query = query.Replace("#SUB_MATERIALS_CONDITION", $"AND CHARINDEX('{materials}', A.MTR_NM) > 0 ");

            query = GetCommonConditionQuery(query, opt);
            query = CreateColumns(query, RItem);

            DataSet ds = new DataSet();
            SqlDBManager.Instance.ExecuteDsQuery(ds, query);

            return ds;
        }

        /// <summary>
        /// RawData Main Data 조회
        /// </summary>
        /// <param name="opt"></param>
        /// <returns></returns>
        public DataSet Get_RawDataMainData(string RItem, SearchOption opt)
        {
            string query = @"
WITH RAW_TBL AS
(
	SELECT A.PLAN_DT,B.PROD_GB AS EQP_ID, A.REG_DT, A.JOB_NO, A.JOB_HG, A.MTR_CD, A.MTR_NM, A.DWG_NO,
		   #BASE AS tVal, 
		   #TOLERANCE AS inRange,
		   #RESULT_VALUE AS R_#ITEM,
		   #RESULT_JUDGE AS JUDGE
	FROM [TKEK_PRD].[dbo].TB_JOB_WORK_PP_SPC A
    INNER JOIN [TKEK_PRD].[dbo].TB_JOB_WORK_PP B
	ON A.PLAN_SEQ = B.PLAN_SEQ AND A.PLAN_DT = B.PLAN_DT
	WHERE 1 = 1
	AND A.PLAN_DT BETWEEN '#START_TIME' AND '#END_TIME'
    --JUDGE #JUDGE_CONDITION
    --LR #LR_CONDITION
    --TYPE #TYPE_CONDITION
    --MODEL #MODEL_CONDITION
    --OPENING #OPENING_CONDITION
    --MATERIALS #MATERIALS_CONDITION
    --EQPID #EQPID_CONDITION
)
SELECT
PLAN_DT, REG_DT, EQP_ID, JOB_NO, JOB_HG, MTR_CD, MTR_NM, DWG_NO,
tVal, ROUND(R_#ITEM, 2) as R_#ITEM,
ROUND(R_#ITEM - tval, 2) as gap,
inRange
FROM raw_tbl
WHERE 1 =1 
ORDER BY PLAN_DT
";

            string replaceQuery = string.Empty;

            replaceQuery = GetCommonConditionQuery(query, opt);
            replaceQuery = CreateColumns(replaceQuery, RItem);

            DataSet ds = new DataSet();
            SqlDBManager.Instance.ExecuteDsQuery(ds, replaceQuery);

            return ds;
        }

        /// <summary>
        /// Trend Chart Main Data 조회
        /// </summary>
        /// <param name="opt"></param>
        /// <returns></returns>
        public DataSet Get_TrendChartMainData(string RItem, SearchOption opt)
        {
            string query = @"
WITH
RAW_TBL AS
(
	SELECT A.PLAN_DT, A.REG_DT, A.JOB_NO, A.JOB_HG, A.MTR_NM, A.DWG_NO,
		   #BASE AS tVal, 
		   #TOLERANCE AS inRange,
		   #RESULT_VALUE AS R_#ITEM,
		   #RESULT_JUDGE AS JUDGE
	FROM [TKEK_PRD].[dbo].TB_JOB_WORK_PP_SPC A
    INNER JOIN [TKEK_PRD].[dbo].TB_JOB_WORK_PP B
	ON A.PLAN_SEQ = B.PLAN_SEQ AND A.PLAN_DT = B.PLAN_DT
	WHERE 1 = 1
    AND A.PLAN_DT BETWEEN '#START_TIME' AND '#END_TIME'
    --JUDGE #JUDGE_CONDITION
    --LR #LR_CONDITION
    --TYPE #TYPE_CONDITION
    --MODEL #MODEL_CONDITION
    --OPENING #OPENING_CONDITION
    --MATERIALS #MATERIALS_CONDITION
    --EQPID #EQPID_CONDITION
)
SELECT TOP #NUMBER PLAN_DT, REG_DT, JOB_NO, JOB_HG, MTR_NM, tVal,
       ROUND(R_#ITEM, 2) AS R_#ITEM,
	   ROUND(R_#ITEM - tVal , 2) AS GAP, 
	   inRange
FROM RAW_TBL
WHERE 1 = 1
";

            query = query.Replace("#NUMBER", opt.TOPN);
            query = GetCommonConditionQuery(query, opt);
            query = CreateColumns(query, RItem);

            DataSet ds = new DataSet();
            SqlDBManager.Instance.ExecuteDsQuery(ds, query);

            return ds;
        }

        /// <summary>
        /// X Chart Main Data 조회
        /// </summary>
        /// <param name="opt"></param>
        /// <returns></returns>
        public DataSet Get_XChartMainData(string RItem, SearchOption opt)
        {
            string query = @"
WITH
RAW_TBL AS
(
	SELECT A.PLAN_DT, CONVERT(VARCHAR(16), A.REG_DT, 120) AS REG_DT, A.JOB_NO, A.JOB_HG, A.MTR_NM, A.DWG_NO,
		   #BASE AS tVal, 
		   #TOLERANCE AS inRange,
		   #RESULT_VALUE AS R_#ITEM,
		   #RESULT_JUDGE AS JUDGE
	FROM [TKEK_PRD].[dbo].TB_JOB_WORK_PP_SPC A
    INNER JOIN [TKEK_PRD].[dbo].TB_JOB_WORK_PP B
	ON A.PLAN_SEQ = B.PLAN_SEQ AND A.PLAN_DT = B.PLAN_DT
	WHERE 1 = 1
    AND A.PLAN_DT BETWEEN '#START_TIME' AND '#END_TIME'
    --JUDGE #JUDGE_CONDITION
    --LR #LR_CONDITION
    --TYPE #TYPE_CONDITION
    --MODEL #MODEL_CONDITION
    --OPENING #OPENING_CONDITION
    --MATERIALS #MATERIALS_CONDITION
    --EQPID #EQPID_CONDITION
)
SELECT TOP #NUMBER PLAN_DT, REG_DT, JOB_NO, JOB_HG, MTR_NM, tVal,
       ROUND(R_#ITEM, 2) AS R_#ITEM,
	   ROUND(R_#ITEM - tVal , 2) AS GAP, 
	   inRange
FROM RAW_TBL
WHERE 1 = 1
";

            query = query.Replace("#NUMBER", (Convert.ToInt32(opt.TOPN) * Convert.ToInt64(opt.TOPN)).ToString());
            query = GetCommonConditionQuery(query, opt);
            query = CreateColumns(query, RItem);

            DataSet ds = new DataSet();
            SqlDBManager.Instance.ExecuteDsQuery(ds, query);

            return ds;
        }

        /// <summary>
        /// Trend Chart Sub Data 조회
        /// </summary>
        /// <param name="opt"></param>
        /// <returns></returns>
        public DataSet Get_TrendChartSubData(string RItem, SearchOption opt, string materials)
        {
            string query = @"
WITH
RAW_TBL AS
(
	SELECT A.PLAN_DT, A.REG_DT, A.JOB_NO, A.JOB_HG, A.MTR_NM, A.DWG_NO,
		   #BASE AS tVal, 
		   #TOLERANCE AS inRange,
		   #RESULT_VALUE AS R_#ITEM,
		   #RESULT_JUDGE AS JUDGE
	FROM [TKEK_PRD].[dbo].TB_JOB_WORK_PP_SPC A
    INNER JOIN [TKEK_PRD].[dbo].TB_JOB_WORK_PP B
	ON A.PLAN_SEQ = B.PLAN_SEQ AND A.PLAN_DT = B.PLAN_DT
	WHERE 1 = 1
    AND A.PLAN_DT BETWEEN '#START_TIME' AND '#END_TIME'
    --JUDGE #JUDGE_CONDITION
    --LR #LR_CONDITION
    --TYPE #TYPE_CONDITION
    --MODEL #MODEL_CONDITION
    --OPENING #OPENING_CONDITION
    --SUB_MATERIALS #SUB_MATERIALS_CONDITION
    --EQPID #EQPID_CONDITION
)
SELECT TOP #NUMBER PLAN_DT, REG_DT, JOB_NO, JOB_HG, MTR_NM, tVal,
       ROUND(R_#ITEM, 2) AS R_#ITEM,
	   ROUND(R_#ITEM - tVal , 2) AS GAP, 
	   inRange
FROM RAW_TBL
WHERE 1 = 1
";
            query = query.Replace("--SUB_MATERIALS", "");
            query = query.Replace("#SUB_MATERIALS_CONDITION", $"AND CHARINDEX('{materials}', A.MTR_NM) > 0 ");

            query = query.Replace("#NUMBER", opt.TOPN);
            query = GetCommonConditionQuery(query, opt);
            query = CreateColumns(query, RItem);

            DataSet ds = new DataSet();
            SqlDBManager.Instance.ExecuteDsQuery(ds, query);

            return ds;
        }

        /// <summary>
        /// X Chart Sub Data 조회
        /// </summary>
        /// <param name="opt"></param>
        /// <returns></returns>
        public DataSet Get_XChartSubData(string RItem, SearchOption opt, string materials)
        {
            string query = @"
WITH
RAW_TBL AS
(
	SELECT A.PLAN_DT, CONVERT(VARCHAR(16), A.REG_DT, 120) AS REG_DT, A.JOB_NO, A.JOB_HG, A.MTR_NM, A.DWG_NO,
		   #BASE AS tVal, 
		   #TOLERANCE AS inRange,
		   #RESULT_VALUE AS R_#ITEM,
		   #RESULT_JUDGE AS JUDGE
	FROM [TKEK_PRD].[dbo].TB_JOB_WORK_PP_SPC A
    INNER JOIN [TKEK_PRD].[dbo].TB_JOB_WORK_PP B
	ON A.PLAN_SEQ = B.PLAN_SEQ AND A.PLAN_DT = B.PLAN_DT
	WHERE 1 = 1
    AND A.PLAN_DT BETWEEN '#START_TIME' AND '#END_TIME'
    --JUDGE #JUDGE_CONDITION
    --LR #LR_CONDITION
    --TYPE #TYPE_CONDITION
    --MODEL #MODEL_CONDITION
    --OPENING #OPENING_CONDITION
    --SUB_MATERIALS #SUB_MATERIALS_CONDITION
    --EQPID #EQPID_CONDITION
)
SELECT TOP #NUMBER PLAN_DT, REG_DT, JOB_NO, JOB_HG, MTR_NM, tVal,
       ROUND(R_#ITEM, 2) AS R_#ITEM,
	   ROUND(R_#ITEM - tVal , 2) AS GAP, 
	   inRange
FROM RAW_TBL
WHERE 1 = 1
";
            query = query.Replace("--SUB_MATERIALS", "");
            query = query.Replace("#SUB_MATERIALS_CONDITION", $"AND CHARINDEX('{materials}', A.MTR_NM) > 0 ");

            query = query.Replace("#NUMBER", (Convert.ToInt32(opt.TOPN) * Convert.ToInt64(opt.TOPN)).ToString());
            query = GetCommonConditionQuery(query, opt);
            query = CreateColumns(query, RItem);

            DataSet ds = new DataSet();
            SqlDBManager.Instance.ExecuteDsQuery(ds, query);

            return ds;
        }

        /// <summary>
        /// X Chart Opening Data 조회
        /// </summary>
        /// <param name="opt"></param>
        /// <returns></returns>
        public DataSet Get_XChartOpeningData(string RItem, SearchOption opt, string opening, string materials)
        {
            string query = @"
WITH
RAW_TBL AS
(
	SELECT A.PLAN_DT, CONVERT(VARCHAR(16), A.REG_DT, 120) AS REG_DT, A.JOB_NO, A.JOB_HG, A.MTR_NM, A.DWG_NO,
		   #BASE AS tVal, 
		   #TOLERANCE AS inRange,
		   #RESULT_VALUE AS R_#ITEM,
		   #RESULT_JUDGE AS JUDGE
	FROM [TKEK_PRD].[dbo].TB_JOB_WORK_PP_SPC A
    INNER JOIN [TKEK_PRD].[dbo].TB_JOB_WORK_PP B
	ON A.PLAN_SEQ = B.PLAN_SEQ AND A.PLAN_DT = B.PLAN_DT
	WHERE 1 = 1
    AND A.PLAN_DT BETWEEN '#START_TIME' AND '#END_TIME'
    --JUDGE #JUDGE_CONDITION
    --LR #LR_CONDITION
    --TYPE #TYPE_CONDITION
    --MODEL #MODEL_CONDITION
    --OPENING #OPENING_CONDITION
    --SUB_OPENING #SUB_OPENING_CONDITION
    --SUB_MATERIALS #SUB_MATERIALS_CONDITION
    --EQPID #EQPID_CONDITION
)
SELECT TOP #NUMBER PLAN_DT, REG_DT, JOB_NO, JOB_HG, MTR_NM, tVal,
       ROUND(R_#ITEM, 2) AS R_#ITEM,
	   ROUND(R_#ITEM - tVal , 2) AS GAP, 
	   inRange
FROM RAW_TBL
WHERE 1 = 1
";
            query = query.Replace("--SUB_OPENING", "");
            query = query.Replace("#SUB_OPENING_CONDITION", $"AND CHARINDEX('{opening}', A.MTR_NM) > 0 ");

            query = query.Replace("--SUB_MATERIALS", "");
            query = query.Replace("#SUB_MATERIALS_CONDITION", $"AND CHARINDEX('{materials}', A.MTR_NM) > 0 ");

            query = query.Replace("#NUMBER", (Convert.ToInt32(opt.TOPN) * Convert.ToInt64(opt.TOPN)).ToString());
            query = GetCommonConditionQuery(query, opt);
            query = CreateColumns(query, RItem);

            DataSet ds = new DataSet();
            SqlDBManager.Instance.ExecuteDsQuery(ds, query);

            return ds;
        }


        /// <summary>
        /// 양품률, 불량률 Main Data 조회
        /// </summary>
        /// <param name="opt"></param>
        /// <returns></returns>
        public DataSet Get_YieldMainData(string RItem, SearchOption opt)
        {
            string query = @"
WITH
RESULT_TBL AS
(
   SELECT SUBSTRING(TBL.Materials, CHARINDEX(' ', TBL.Materials), LEN(TBL.Materials))  AS MATERIALS
   FROM 
   (
	SELECT SUBSTRING(A.MTR_NM, CHARINDEX('CO', A.MTR_NM), LEN(A.MTR_NM)) AS Materials , #RESULT_JUDGE
	FROM [TKEK_PRD].[dbo].TB_JOB_WORK_PP_SPC A
    INNER JOIN [TKEK_PRD].[dbo].TB_JOB_WORK_PP B
	ON A.PLAN_SEQ = B.PLAN_SEQ AND A.PLAN_DT = B.PLAN_DT
	WHERE 1 = 1
    AND A.PLAN_DT BETWEEN '#START_TIME' AND '#END_TIME'
    --LR #LR_CONDITION
    --TYPE #TYPE_CONDITION
    --MODEL #MODEL_CONDITION
    --OPENING #OPENING_CONDITION
    --MATERIALS #MATERIALS_CONDITION
    --EQPID #EQPID_CONDITION
   ) TBL
   WHERE 1 = 1
   GROUP BY SUBSTRING(TBL.Materials, CHARINDEX(' ', TBL.Materials), LEN(TBL.Materials))
),
OK_TBL AS
(
	SELECT SUBSTRING(TBL.Materials, CHARINDEX(' ', TBL.Materials), LEN(TBL.Materials))  AS MATERIALS, COUNT(*) AS CNT
    FROM 
    (
      SELECT SUBSTRING(A.MTR_NM, CHARINDEX('CO', A.MTR_NM), LEN(A.MTR_NM)) AS Materials , #RESULT_JUDGE
	  FROM [TKEK_PRD].[dbo].TB_JOB_WORK_PP_SPC A
      INNER JOIN [TKEK_PRD].[dbo].TB_JOB_WORK_PP B
	  ON A.PLAN_SEQ = B.PLAN_SEQ AND A.PLAN_DT = B.PLAN_DT
      WHERE 1 = 1
      AND A.PLAN_DT BETWEEN '#START_TIME' AND '#END_TIME'
      AND #RESULT_JUDGE = 'OK'
      --LR #LR_CONDITION
      --TYPE #TYPE_CONDITION
      --MODEL #MODEL_CONDITION
      --OPENING #OPENING_CONDITION
      --MATERIALS #MATERIALS_CONDITION
      --EQPID #EQPID_CONDITION
    ) TBL
    WHERE 1 = 1
    GROUP BY SUBSTRING(TBL.Materials, CHARINDEX(' ', TBL.Materials), LEN(TBL.Materials))
),
NG_TBL AS
(
	   SELECT SUBSTRING(TBL.Materials, CHARINDEX(' ', TBL.Materials), LEN(TBL.Materials))  AS MATERIALS, COUNT(*) AS CNT
	   FROM 
	   (
		SELECT SUBSTRING(A.MTR_NM, CHARINDEX('CO', A.MTR_NM), LEN(A.MTR_NM)) AS Materials , #RESULT_JUDGE
	    FROM [TKEK_PRD].[dbo].TB_JOB_WORK_PP_SPC A
        INNER JOIN [TKEK_PRD].[dbo].TB_JOB_WORK_PP B
	    ON A.PLAN_SEQ = B.PLAN_SEQ AND A.PLAN_DT = B.PLAN_DT
        WHERE 1 = 1
		AND A.PLAN_DT BETWEEN '#START_TIME' AND '#END_TIME'
        AND #RESULT_JUDGE = 'NG'
        --LR #LR_CONDITION
        --TYPE #TYPE_CONDITION
        --MODEL #MODEL_CONDITION
        --OPENING #OPENING_CONDITION
        --MATERIALS #MATERIALS_CONDITION
        --EQPID #EQPID_CONDITION
	   ) TBL
	   WHERE 1 = 1
	   GROUP BY SUBSTRING(TBL.Materials, CHARINDEX(' ', TBL.Materials), LEN(TBL.Materials))
),
TOTAL_OK_TBL AS
(
      SELECT COUNT(*) AS OK_TOTAL_CNT
	  FROM [TKEK_PRD].[dbo].TB_JOB_WORK_PP_SPC A
      INNER JOIN [TKEK_PRD].[dbo].TB_JOB_WORK_PP B
	  ON A.PLAN_SEQ = B.PLAN_SEQ AND A.PLAN_DT = B.PLAN_DT
      WHERE 1 = 1
		AND A.PLAN_DT BETWEEN '#START_TIME' AND '#END_TIME'
        AND A.RESULT_OK = 'OK'
        --LR #LR_CONDITION
        --TYPE #TYPE_CONDITION
        --MODEL #MODEL_CONDITION
        --OPENING #OPENING_CONDITION
        --MATERIALS #MATERIALS_CONDITION
        --EQPID #EQPID_CONDITION
),
TOTAL_NG_TBL AS
(
		SELECT COUNT(*) AS NG_TOTAL_CNT
	    FROM [TKEK_PRD].[dbo].TB_JOB_WORK_PP_SPC A
        INNER JOIN [TKEK_PRD].[dbo].TB_JOB_WORK_PP B
	    ON A.PLAN_SEQ = B.PLAN_SEQ AND A.PLAN_DT = B.PLAN_DT
        WHERE 1 = 1
		AND A.PLAN_DT BETWEEN '#START_TIME' AND '#END_TIME'
        AND A.RESULT_OK = 'NG'
        --LR #LR_CONDITION
        --TYPE #TYPE_CONDITION
        --MODEL #MODEL_CONDITION
        --OPENING #OPENING_CONDITION
        --MATERIALS #MATERIALS_CONDITION
        --EQPID #EQPID_CONDITION
)
SELECT TOP #NUMBER RESULT.MATERIALS, OK_TOTAL.OK_TOTAL_CNT, NG_TOTAL.NG_TOTAL_CNT,
	   CONVERT(numeric, ISNULL(OK.CNT , 0)) AS OK_CNT, 
	   CONVERT(numeric, ISNULL(NG.CNT , 0)) AS NG_CNT , 
	   --#OK ISNULL(ROUND(CONVERT(float, OK.CNT) / CONVERT(float, CONVERT(numeric, ISNULL(OK.CNT , 0)) + CONVERT(numeric, ISNULL(NG.CNT , 0))), 2) * 100, 0) AS '양품률',
       --#NG 100 - ISNULL(ROUND(CONVERT(float, OK.CNT) / CONVERT(float, CONVERT(numeric, ISNULL(OK.CNT , 0)) + CONVERT(numeric, ISNULL(NG.CNT , 0))), 2) * 100, 0) AS '불량률',
       --#OK CONVERT(numeric, ISNULL(OK.CNT , 0)) AS TOTAL
       --#NG CONVERT(numeric, ISNULL(NG.CNT , 0)) AS TOTAL
FROM RESULT_TBL RESULT
LEFT JOIN OK_TBL OK
ON RESULT.MATERIALS = OK.MATERIALS
LEFT JOIN NG_TBL NG
ON RESULT.MATERIALS = NG.MATERIALS
LEFT JOIN TOTAL_OK_TBL OK_TOTAL
ON 1 = 1
LEFT JOIN TOTAL_NG_TBL NG_TOTAL
ON 1 = 1
WHERE 1 = 1
ORDER BY TOTAL DESC
";
            query = query.Replace("#NUMBER", opt.TOPN);

            //OK이면 양품률 보여주기
            if (opt.JUDGE.Contains("OK"))
            {
                query = query.Replace("--#OK", "");
            }
            else //NG 이면 불량률 보여주기
            {
                query = query.Replace("--#NG", "");
            }

            query = GetCommonConditionQuery(query, opt);
            query = CreateColumns(query, RItem);

            DataSet ds = new DataSet();
            SqlDBManager.Instance.ExecuteDsQuery(ds, query);

            return ds;
        }

        /// <summary>
        /// 양품률 Sub Data 조회
        /// </summary>
        /// <param name="opt"></param>
        /// <returns></returns>
        public DataSet Get_YieldSubData(string RItem, SearchOption opt, string materials)
        {
            string query = @"
WITH
RESULT_TBL AS
(
   SELECT Opening, COUNT(*) AS CNT
   FROM 
   (
	SELECT SUBSTRING(A.Materials, 0, CHARINDEX('*', A.Materials)) AS Opening
	FROM
	(
		SELECT SUBSTRING(A.MTR_NM, CHARINDEX('CO', A.MTR_NM), LEN(A.MTR_NM)) AS Materials
		FROM [TKEK_PRD].[dbo].TB_JOB_WORK_PP_SPC A
		INNER JOIN [TKEK_PRD].[dbo].TB_JOB_WORK_PP B
		ON A.PLAN_SEQ = B.PLAN_SEQ AND A.PLAN_DT = B.PLAN_DT
		WHERE 1 = 1
    AND A.PLAN_DT BETWEEN '#START_TIME' AND '#END_TIME'
    --LR #LR_CONDITION
    --TYPE #TYPE_CONDITION
    --MODEL #MODEL_CONDITION
    --OPENING #OPENING_CONDITION
    --SUB_MATERIALS #SUB_MATERIALS_CONDITION
    --EQPID #EQPID_CONDITION
	   ) A
   ) TBL
   WHERE 1 = 1
   GROUP BY Opening
),
OK_TBL AS
(
	SELECT Opening, COUNT(*) AS CNT
   FROM 
   (
	SELECT SUBSTRING(A.Materials, 0, CHARINDEX('*', A.Materials)) AS Opening
	FROM
	(
		SELECT SUBSTRING(A.MTR_NM, CHARINDEX('CO', A.MTR_NM), LEN(A.MTR_NM)) AS Materials
		FROM [TKEK_PRD].[dbo].TB_JOB_WORK_PP_SPC A
		INNER JOIN [TKEK_PRD].[dbo].TB_JOB_WORK_PP B
		ON A.PLAN_SEQ = B.PLAN_SEQ AND A.PLAN_DT = B.PLAN_DT
		WHERE 1 = 1
      AND A.PLAN_DT BETWEEN '#START_TIME' AND '#END_TIME'
      AND #RESULT_JUDGE = 'OK'
      --LR #LR_CONDITION
      --TYPE #TYPE_CONDITION
      --MODEL #MODEL_CONDITION
      --OPENING #OPENING_CONDITION
      --SUB_MATERIALS #SUB_MATERIALS_CONDITION
      --EQPID #EQPID_CONDITION
	   ) A
   ) TBL
   WHERE 1 = 1
   GROUP BY Opening
),
NG_TBL AS
(
	   SELECT Opening, COUNT(*) AS CNT
   FROM 
   (
	SELECT SUBSTRING(A.Materials, 0, CHARINDEX('*', A.Materials)) AS Opening
	FROM
	(
		SELECT SUBSTRING(A.MTR_NM, CHARINDEX('CO', A.MTR_NM), LEN(A.MTR_NM)) AS Materials
		FROM [TKEK_PRD].[dbo].TB_JOB_WORK_PP_SPC A
		INNER JOIN [TKEK_PRD].[dbo].TB_JOB_WORK_PP B
		ON A.PLAN_SEQ = B.PLAN_SEQ AND A.PLAN_DT = B.PLAN_DT
		WHERE 1 = 1
		AND A.PLAN_DT BETWEEN '#START_TIME' AND '#END_TIME'
        AND #RESULT_JUDGE = 'NG'
        --LR #LR_CONDITION
        --TYPE #TYPE_CONDITION
        --MODEL #MODEL_CONDITION
        --OPENING #OPENING_CONDITION
        --SUB_MATERIALS #SUB_MATERIALS_CONDITION
        --EQPID #EQPID_CONDITION
	   ) A
   ) TBL
   WHERE 1 = 1
   GROUP BY Opening
),
TOTAL_OK_TBL AS
(
      SELECT COUNT(*) AS OK_TOTAL_CNT
	  FROM [TKEK_PRD].[dbo].TB_JOB_WORK_PP_SPC A
      INNER JOIN [TKEK_PRD].[dbo].TB_JOB_WORK_PP B
	  ON A.PLAN_SEQ = B.PLAN_SEQ AND A.PLAN_DT = B.PLAN_DT
      WHERE 1 = 1
		AND A.PLAN_DT BETWEEN '#START_TIME' AND '#END_TIME'
        AND A.RESULT_OK = 'OK'
        --LR #LR_CONDITION
        --TYPE #TYPE_CONDITION
        --MODEL #MODEL_CONDITION
        --OPENING #OPENING_CONDITION
        --SUB_MATERIALS #SUB_MATERIALS_CONDITION
        --EQPID #EQPID_CONDITION
),
TOTAL_NG_TBL AS
(
		SELECT COUNT(*) AS NG_TOTAL_CNT
	    FROM [TKEK_PRD].[dbo].TB_JOB_WORK_PP_SPC A
        INNER JOIN [TKEK_PRD].[dbo].TB_JOB_WORK_PP B
	    ON A.PLAN_SEQ = B.PLAN_SEQ AND A.PLAN_DT = B.PLAN_DT
        WHERE 1 = 1
		AND A.PLAN_DT BETWEEN '#START_TIME' AND '#END_TIME'
        AND A.RESULT_OK = 'NG'
        --LR #LR_CONDITION
        --TYPE #TYPE_CONDITION
        --MODEL #MODEL_CONDITION
        --OPENING #OPENING_CONDITION
        --SUB_MATERIALS #SUB_MATERIALS_CONDITION
        --EQPID #EQPID_CONDITION
)
SELECT TOP #NUMBER RESULT.Opening, OK_TOTAL.OK_TOTAL_CNT, NG_TOTAL.NG_TOTAL_CNT,
	   CONVERT(numeric, ISNULL(OK.CNT , 0)) AS OK_CNT, 
	   CONVERT(numeric, ISNULL(NG.CNT , 0)) AS NG_CNT , 
	   --#OK ISNULL(ROUND(CONVERT(float, OK.CNT) / CONVERT(float, CONVERT(numeric, ISNULL(OK.CNT , 0)) + CONVERT(numeric, ISNULL(NG.CNT , 0))), 2) * 100, 0) AS '양품률'
       --#NG 100 - ISNULL(ROUND(CONVERT(float, OK.CNT) / CONVERT(float, CONVERT(numeric, ISNULL(OK.CNT , 0)) + CONVERT(numeric, ISNULL(NG.CNT , 0))), 2) * 100, 0) AS '불량률'
FROM RESULT_TBL RESULT
LEFT JOIN OK_TBL OK
ON RESULT.Opening = OK.Opening
LEFT JOIN NG_TBL NG
ON RESULT.Opening = NG.Opening
LEFT JOIN TOTAL_OK_TBL OK_TOTAL
ON 1 = 1
LEFT JOIN TOTAL_NG_TBL NG_TOTAL
ON 1 = 1
ORDER BY 4 DESC
";
            query = query.Replace("--SUB_MATERIALS", "");
            query = query.Replace("#SUB_MATERIALS_CONDITION", $"AND CHARINDEX('{materials}', A.MTR_NM) > 0 ");

            query = query.Replace("#NUMBER", opt.TOPN);

            //OK이면 양품률 보여주기
            if (opt.JUDGE.Contains("OK"))
            {
                query = query.Replace("--#OK", "");
            }
            else //NG 이면 불량률 보여주기
            {
                query = query.Replace("--#NG", "");
            }

            query = GetCommonConditionQuery(query, opt);
            query = CreateColumns(query, RItem);

            DataSet ds = new DataSet();
            SqlDBManager.Instance.ExecuteDsQuery(ds, query);

            return ds;
        }

        /// <summary>
        /// 양품률 Opening Data 조회
        /// </summary>
        /// <param name="opt"></param>
        /// <returns></returns>
        public DataSet Get_YieldOpeningData(string RItem, SearchOption opt, string opening, string materials)
        {
            string query = @"
WITH
RESULT_TBL AS
(
   SELECT Opening, COUNT(*) AS CNT
   FROM 
   (
	SELECT SUBSTRING(A.Materials, 0, CHARINDEX('*', A.Materials)) AS Opening
	FROM
	(
		SELECT SUBSTRING(A.MTR_NM, CHARINDEX('CO', A.MTR_NM), LEN(A.MTR_NM)) AS Materials
		FROM [TKEK_PRD].[dbo].TB_JOB_WORK_PP_SPC A
		INNER JOIN [TKEK_PRD].[dbo].TB_JOB_WORK_PP B
		ON A.PLAN_SEQ = B.PLAN_SEQ AND A.PLAN_DT = B.PLAN_DT
		WHERE 1 = 1
    AND A.PLAN_DT BETWEEN '#START_TIME' AND '#END_TIME'
    --LR #LR_CONDITION
    --TYPE #TYPE_CONDITION
    --MODEL #MODEL_CONDITION
    --OPENING #OPENING_CONDITION
    --SUB_OPENING #SUB_OPENING_CONDITION
    --SUB_MATERIALS #SUB_MATERIALS_CONDITION
    --EQPID #EQPID_CONDITION
	   ) A
   ) TBL
   WHERE 1 = 1
   GROUP BY Opening
),
OK_TBL AS
(
	SELECT Opening, COUNT(*) AS CNT
   FROM 
   (
	SELECT SUBSTRING(A.Materials, 0, CHARINDEX('*', A.Materials)) AS Opening
	FROM
	(
		SELECT SUBSTRING(A.MTR_NM, CHARINDEX('CO', A.MTR_NM), LEN(A.MTR_NM)) AS Materials
		FROM [TKEK_PRD].[dbo].TB_JOB_WORK_PP_SPC A
		INNER JOIN [TKEK_PRD].[dbo].TB_JOB_WORK_PP B
		ON A.PLAN_SEQ = B.PLAN_SEQ AND A.PLAN_DT = B.PLAN_DT
		WHERE 1 = 1
      AND A.PLAN_DT BETWEEN '#START_TIME' AND '#END_TIME'
      AND #RESULT_JUDGE = 'OK'
      --LR #LR_CONDITION
      --TYPE #TYPE_CONDITION
      --MODEL #MODEL_CONDITION
      --OPENING #OPENING_CONDITION
      --SUB_OPENING #SUB_OPENING_CONDITION
      --SUB_MATERIALS #SUB_MATERIALS_CONDITION
      --EQPID #EQPID_CONDITION
	   ) A
   ) TBL
   WHERE 1 = 1
   GROUP BY Opening
),
NG_TBL AS
(
	   SELECT Opening, COUNT(*) AS CNT
   FROM 
   (
	SELECT SUBSTRING(A.Materials, 0, CHARINDEX('*', A.Materials)) AS Opening
	FROM
	(
		SELECT SUBSTRING(A.MTR_NM, CHARINDEX('CO', A.MTR_NM), LEN(A.MTR_NM)) AS Materials
		FROM [TKEK_PRD].[dbo].TB_JOB_WORK_PP_SPC A
		INNER JOIN [TKEK_PRD].[dbo].TB_JOB_WORK_PP B
		ON A.PLAN_SEQ = B.PLAN_SEQ AND A.PLAN_DT = B.PLAN_DT
		WHERE 1 = 1
		AND A.PLAN_DT BETWEEN '#START_TIME' AND '#END_TIME'
        AND #RESULT_JUDGE = 'NG'
        --LR #LR_CONDITION
        --TYPE #TYPE_CONDITION
        --MODEL #MODEL_CONDITION
        --OPENING #OPENING_CONDITION
        --SUB_OPENING #SUB_OPENING_CONDITION
        --SUB_MATERIALS #SUB_MATERIALS_CONDITION
        --EQPID #EQPID_CONDITION
	   ) A
   ) TBL
   WHERE 1 = 1
   GROUP BY Opening
),
TOTAL_OK_TBL AS
(
      SELECT COUNT(*) AS OK_TOTAL_CNT
	  FROM [TKEK_PRD].[dbo].TB_JOB_WORK_PP_SPC A
      INNER JOIN [TKEK_PRD].[dbo].TB_JOB_WORK_PP B
	  ON A.PLAN_SEQ = B.PLAN_SEQ AND A.PLAN_DT = B.PLAN_DT
      WHERE 1 = 1
		AND A.PLAN_DT BETWEEN '#START_TIME' AND '#END_TIME'
        AND A.RESULT_OK = 'OK'
        --LR #LR_CONDITION
        --TYPE #TYPE_CONDITION
        --MODEL #MODEL_CONDITION
        --OPENING #OPENING_CONDITION
        --SUB_OPENING #SUB_OPENING_CONDITION
        --SUB_MATERIALS #SUB_MATERIALS_CONDITION
        --EQPID #EQPID_CONDITION
),
TOTAL_NG_TBL AS
(
		SELECT COUNT(*) AS NG_TOTAL_CNT
	    FROM [TKEK_PRD].[dbo].TB_JOB_WORK_PP_SPC A
        INNER JOIN [TKEK_PRD].[dbo].TB_JOB_WORK_PP B
	    ON A.PLAN_SEQ = B.PLAN_SEQ AND A.PLAN_DT = B.PLAN_DT
        WHERE 1 = 1
		AND A.PLAN_DT BETWEEN '#START_TIME' AND '#END_TIME'
        AND A.RESULT_OK = 'NG'
        --LR #LR_CONDITION
        --TYPE #TYPE_CONDITION
        --MODEL #MODEL_CONDITION
        --OPENING #OPENING_CONDITION
        --SUB_OPENING #SUB_OPENING_CONDITION
        --SUB_MATERIALS #SUB_MATERIALS_CONDITION
        --EQPID #EQPID_CONDITION
)
SELECT TOP #NUMBER RESULT.Opening, OK_TOTAL.OK_TOTAL_CNT, NG_TOTAL.NG_TOTAL_CNT,
	   CONVERT(numeric, ISNULL(OK.CNT , 0)) AS OK_CNT, 
	   CONVERT(numeric, ISNULL(NG.CNT , 0)) AS NG_CNT , 
	   --#OK ISNULL(ROUND(CONVERT(float, OK.CNT) / CONVERT(float, CONVERT(numeric, ISNULL(OK.CNT , 0)) + CONVERT(numeric, ISNULL(NG.CNT , 0))), 2) * 100, 0) AS '양품률'
       --#NG 100 - ISNULL(ROUND(CONVERT(float, OK.CNT) / CONVERT(float, CONVERT(numeric, ISNULL(OK.CNT , 0)) + CONVERT(numeric, ISNULL(NG.CNT , 0))), 2) * 100, 0) AS '불량률'
FROM RESULT_TBL RESULT
LEFT JOIN OK_TBL OK
ON RESULT.Opening = OK.Opening
LEFT JOIN NG_TBL NG
ON RESULT.Opening = NG.Opening
LEFT JOIN TOTAL_OK_TBL OK_TOTAL
ON 1 = 1
LEFT JOIN TOTAL_NG_TBL NG_TOTAL
ON 1 = 1
ORDER BY 4 DESC
";
            query = query.Replace("--SUB_OPENING", "");
            query = query.Replace("#SUB_OPENING_CONDITION", $"AND CHARINDEX('{opening}', A.MTR_NM) > 0 ");

            query = query.Replace("--SUB_MATERIALS", "");
            query = query.Replace("#SUB_MATERIALS_CONDITION", $"AND CHARINDEX('{materials}', A.MTR_NM) > 0 ");

            query = query.Replace("#NUMBER", opt.TOPN);

            //OK이면 양품률 보여주기
            if (opt.JUDGE.Contains("OK"))
            {
                query = query.Replace("--#OK", "");
            }
            else //NG 이면 불량률 보여주기
            {
                query = query.Replace("--#NG", "");
            }

            query = GetCommonConditionQuery(query, opt);
            query = CreateColumns(query, RItem);

            DataSet ds = new DataSet();
            SqlDBManager.Instance.ExecuteDsQuery(ds, query);

            return ds;
        }

        /// <summary>
        /// Search Option 데이터 조회
        /// </summary>
        /// <param name="opt"></param>
        /// <returns></returns>
        public DataSet Get_SearchOptionData(SearchOption opt)
        {
            string query = @"
SELECT DISTINCT REPLACE(A.MTR_NM, ';#', ' ') AS SearchOption
FROM [TKEK_PRD].[dbo].TB_JOB_WORK_PP_SPC A
INNER JOIN [TKEK_PRD].[dbo].TB_JOB_WORK_PP B
ON A.PLAN_SEQ = B.PLAN_SEQ AND A.PLAN_DT = B.PLAN_DT
WHERE 1 = 1
AND A.PLAN_DT BETWEEN '#START_TIME' AND '#END_TIME'
";

            query = query.Replace("#START_TIME", opt.START_TIME.ToString().Substring(0, 8));
            query = query.Replace("#END_TIME", opt.END_TIME.ToString().Substring(0, 8));

            DataSet ds = new DataSet();
            SqlDBManager.Instance.ExecuteDsQuery(ds, query);

            return ds;
        }

        /// <summary>
        /// 설비 데이터 조회
        /// </summary>
        /// <param name="opt"></param>
        /// <returns></returns>
        public DataSet Get_EquipmentSearchOptionData(SearchOption opt)
        {
            string query = @"
SELECT DISTINCT B.PROD_GB
FROM [TKEK_PRD].[dbo].TB_JOB_WORK_PP_SPC A
INNER JOIN [TKEK_PRD].[dbo].TB_JOB_WORK_PP B
ON A.PLAN_SEQ = B.PLAN_SEQ AND A.PLAN_DT = B.PLAN_DT
WHERE 1 = 1
AND A.PLAN_DT BETWEEN '#START_TIME' AND '#END_TIME'
ORDER BY B.PROD_GB ASC
";

            query = query.Replace("#START_TIME", opt.START_TIME.ToString().Substring(0, 8));
            query = query.Replace("#END_TIME", opt.END_TIME.ToString().Substring(0, 8));

            DataSet ds = new DataSet();
            SqlDBManager.Instance.ExecuteDsQuery(ds, query);

            return ds;
        }

        /// <summary>
        /// SearchOption Query 생성 메서드
        /// </summary>
        /// <param name="replaceQuery"></param>
        /// <param name="opt"></param>
        /// <returns></returns>
        private string GetCommonConditionQuery(string replaceQuery, SearchOption opt)
        {
            string query = string.Empty;
            query = replaceQuery;

            //DateTime 설정
            query = query.Replace("#START_TIME", opt.START_TIME.Substring(0, 8).ToString());
            query = query.Replace("#END_TIME", opt.END_TIME.Substring(0, 8).ToString());

            //JUDGE 설정
            query = query.Replace("--JUDGE", "");
            query = query.Replace("#JUDGE_CONDITION", "AND A.#RESULT_JUDGE = '#OK_NG'");
            query = query.Replace("#OK_NG", opt.JUDGE);

            //DOOR 설정
            query = SearchOptionQuery(query, opt.DOOR, "--DOOR", "#DOOR_CONDITION");

            //LR 설정
            query = SearchOptionQuery(query, opt.LR, "--LR", "#LR_CONDITION");

            //TYPE 설정
            query = SearchOptionQuery(query, opt.TYPE, "--TYPE", "#TYPE_CONDITION");

            //MODEL 설정
            query = SearchOptionQuery(query, opt.MODEL, "--MODEL", "#MODEL_CONDITION");

            //OPENING 설정
            query = SearchOptionQuery(query, opt.OPENING, "--OPENING", "#OPENING_CONDITION");

            //MATERIALS 설정
            query = SearchOptionQuery(query, opt.MATERIALS, "--MATERIALS", "#MATERIALS_CONDITION");

            //EQPID 설정
            query = EqpIDQuery(query, opt.EQPID, "--EQPID", "#EQPID_CONDITION");

            return query;
        }

        /// <summary>
        /// 조회 옵션 쿼리 생성 메서드
        /// </summary>
        /// <param name="query"></param>
        /// <param name="option"></param>
        /// <param name="target"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        private string SearchOptionQuery(string query, string option, string target, string condition)
        {
            if (option != "(ALL)")
            {
                query = query.Replace(target, "");
                query = query.Replace(condition, WhereCondition(option));
            }

            return query;
        }

        /// <summary>
        /// EQP ID 쿼리 생성 메서드
        /// </summary>
        /// <param name="query"></param>
        /// <param name="option"></param>
        /// <param name="target"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        private string EqpIDQuery(string query, string option, string target, string condition)
        {
            if (option != "(ALL)")
            {
                query = query.Replace(target, "");
                query = query.Replace(condition, WhereEquipmentCondition(option));
            }

            return query;
        }

        /// <summary>
        /// SearchOption Query Where 조건 생성 메서드
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        private string WhereCondition(string category)
        {
            StringBuilder query = new StringBuilder();

            query.Length = 0;

            query.AppendFormat("AND ( \n");

            string[] arr = category.Split(',');

            for (int idx = 0; idx < arr.Length; idx++)
            {
                if (idx == 0)
                {
                    query.AppendFormat($"CHARINDEX({arr[idx].ToString()}, A.MTR_NM) > 0 \n");
                }
                else if (idx >= 1)
                {
                    query.AppendFormat("OR \n");
                    query.AppendFormat($"CHARINDEX({arr[idx].ToString()}, A.MTR_NM) > 0 \n");
                }
            }

            query.AppendFormat(")");

            return query.ToString();
        }

        /// <summary>
        /// SearchOption Query Where Equipment 조건 생성 메서드
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        private string WhereEquipmentCondition(string category)
        {
            StringBuilder query = new StringBuilder();

            query.Length = 0;

            query.AppendFormat("AND ( \n");

            string[] arr = category.Split(',');

            for (int idx = 0; idx < arr.Length; idx++)
            {
                if (idx == 0)
                {
                    query.AppendFormat($"CHARINDEX({arr[idx].ToString()}, B.PROD_GB) > 0 \n");
                }
                else if (idx >= 1)
                {
                    query.AppendFormat("OR \n");
                    query.AppendFormat($"CHARINDEX({arr[idx].ToString()}, B.PROD_GB) > 0 \n");
                }
            }

            query.AppendFormat(")");

            return query.ToString();
        }

        /// <summary>
        /// Result Item 에 따른 (Width, Angle, Hole_L, Hole_C, Hole_R)
        /// 컬럼 설정 메서드
        /// </summary>
        /// <param name="query"></param>
        /// <param name="RItem"></param>
        /// <returns></returns>
        public string CreateColumns(string query, string RItem)
        {
            string[] resultArr = { "Width", "Angle", "Hole_L", "Hole_C", "Hole_R" };
            string columnName = string.Empty;

            var result = resultArr.Where(s => RItem.Contains(s)).ToList();

            columnName = result[0].ToUpper();

            if (columnName == "WIDTH")
            {
                columnName = "DR_" + columnName;
            }

            query = query.Replace("#ITEM", RItem);
            query = query.Replace("#BASE", columnName + "_BAS"); //BAS 컬럼
            query = query.Replace("#TOLERANCE", columnName + "_TOR"); //TOR 컬럼
            query = query.Replace("#RESULT_VALUE", columnName + "_VAL"); //VAL 컬럼
            query = query.Replace("#RESULT_JUDGE", columnName + "_OK"); // OK 컬럼

            return query;
        }
    }
}
