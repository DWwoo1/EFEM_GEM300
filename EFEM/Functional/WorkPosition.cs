using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Define.DefineConstant;
using Define.DefineEnumProject.Task;
using Define.DefineEnumProject.Map;

namespace FrameOfSystem3
{
    /// <summary>
    /// 작업 순서관련 알고리즘 모음 클래스
    /// </summary>
    static class WorkPosition
    {
        /// <summary>
        /// 해당 Unit No를 전달받은 Subject Parameter를 기반으로 Unit Array Index로 반환한다.
        /// </summary>
        public static void GetUnitIndexByUnitNo(
            int UnitNo,
            IPointXY GroupCount,
            IPointXY UnitCount,
			EN_WORK_START_POINT StartPos,
            EN_WORK_DIRECTION Direction,
            out IPointXY GroupIndex,
            out IPointXY UnitIndex
            )
        {
            int TotalColum = GroupCount.x * UnitCount.x;
            int TotalRow = GroupCount.y * UnitCount.y;
            int FindX = -1;
            int FindY = -1;
            int FindGroupX = -1;
            int FindGroupY = -1;

			if (TotalColum < 1 || TotalRow < 1)
			{
				GroupIndex.x = -1;
				GroupIndex.y = -1;
				UnitIndex.x = -1;
				UnitIndex.y = -1;
				return;
			}

            #region
            switch (Direction)
            {
                case EN_WORK_DIRECTION.HORIZONTAL_ONEWAY:
                    #region
                    switch (StartPos)
                    {
                        case EN_WORK_START_POINT.LEFT_TOP:
                            FindX = UnitNo % TotalColum;
                            FindY = UnitNo / TotalColum;
                            break;
                        case EN_WORK_START_POINT.LEFT_BOTTOM:
                            FindX = UnitNo % TotalRow;
                            FindY = TotalColum - (UnitNo / TotalRow) - 1;
                            break;
                        case EN_WORK_START_POINT.RIGHT_TOP:
                            FindX = (TotalRow - (UnitNo % TotalRow)) - 1;
                            FindY = UnitNo / TotalRow;
                            break;
                        case EN_WORK_START_POINT.RIGHT_BOTTOM:
                            FindX = (TotalRow - (UnitNo % TotalRow)) - 1;
                            FindY = TotalColum - (UnitNo / TotalRow) - 1;
                            break;
                    }
                    #endregion
                    break;
                case EN_WORK_DIRECTION.HORIZONTAL_ZIGZAG:
                    #region
                    switch (StartPos)
                    {
                        case EN_WORK_START_POINT.LEFT_TOP:
                            FindY = UnitNo / TotalRow;
                            if ((FindY % 2) == 0)
                                FindX = UnitNo % TotalRow;
                            else
                                FindX = (TotalRow - (UnitNo % TotalRow)) - 1;
                            break;
                        case EN_WORK_START_POINT.LEFT_BOTTOM:
                            FindY = TotalColum - (UnitNo / TotalRow) - 1;
                            if (((TotalColum - FindY - 1) % 2) == 0)
                                FindX = UnitNo % TotalRow;
                            else
                                FindX = (TotalRow - (UnitNo % TotalRow)) - 1;
                            break;
                        case EN_WORK_START_POINT.RIGHT_TOP:
                            FindY = UnitNo / TotalRow;
                            if ((FindY % 2) == 0)
                                FindX = (TotalRow - (UnitNo % TotalRow)) - 1;
                            else
                                FindX = UnitNo % TotalRow;
                            break;
                        case EN_WORK_START_POINT.RIGHT_BOTTOM:
                            FindY = TotalColum - (UnitNo / TotalRow) - 1;
                            if (((TotalColum - FindY - 1) % 2) == 0)
                                FindX = (TotalRow - (UnitNo % TotalRow)) - 1;
                            else
                                FindX = UnitNo % TotalRow;
                            break;
                    }
                    #endregion
                    break;
                case EN_WORK_DIRECTION.VERTICAL_ONEWAY:
                    #region
                    switch (StartPos)
                    {
                        case EN_WORK_START_POINT.LEFT_TOP:
                            FindX = UnitNo / TotalColum;
                            FindY = UnitNo % TotalColum;
                            break;
                        case EN_WORK_START_POINT.LEFT_BOTTOM:
                            FindX = UnitNo / TotalColum;
                            FindY = TotalColum - (UnitNo % TotalColum) - 1;
                            break;
                        case EN_WORK_START_POINT.RIGHT_TOP:
                            FindX = TotalRow - (UnitNo / TotalColum) - 1;
                            FindY = UnitNo % TotalColum;
                            break;
                        case EN_WORK_START_POINT.RIGHT_BOTTOM:
                            FindX = TotalRow - (UnitNo / TotalColum) - 1;
                            FindY = (TotalColum - (UnitNo % TotalColum)) - 1;
                            break;
                    }
                    #endregion
                    break;
                case EN_WORK_DIRECTION.VERTICAL_ZIGZAG:
                    #region
                    switch (StartPos)
                    {
                        case EN_WORK_START_POINT.LEFT_TOP:
                            FindX = (UnitNo / TotalColum);
                            if ((UnitNo / (TotalRow * TotalColum) % 2) == 1 && (GroupCount.x % 2) != 0)
                            {
                                if ((FindX % 2) == 1)
                                    FindY = UnitNo % TotalColum;
                                else
                                    FindY = (TotalColum - (UnitNo % TotalColum)) - 1;
                            }
                            else
                            {
                                if ((FindX % 2) == 0)
                                    FindY = UnitNo % TotalColum;
                                else
                                    FindY = (TotalColum - (UnitNo % TotalColum)) - 1;
                            }
                            break;
                        case EN_WORK_START_POINT.LEFT_BOTTOM:
                            FindX = UnitNo / TotalColum;
                            if ((UnitNo / (TotalRow * TotalColum) % 2) == 1 && (GroupCount.x % 2) != 0)
                            {
                                if (((TotalRow - FindX - 1) % 2) == 1)
                                    FindY = UnitNo % TotalColum;
                                else
                                    FindY = (TotalColum - (UnitNo % TotalColum)) - 1;
                            }
                            else
                            {
                                if ((FindX % 2) == 0)
                                    FindY = TotalColum - (UnitNo % TotalColum) - 1;
                                else
                                    FindY = UnitNo % TotalColum;
                            }
                            break;
                        case EN_WORK_START_POINT.RIGHT_TOP:
                            FindX = TotalRow - (UnitNo / TotalColum) - 1;
                            if ((UnitNo / (TotalRow * TotalColum) % 2) == 1 && (FindX % 2) != 0)
                            {
                                if (((TotalRow - FindX - 1) % 2) == 1)
                                    FindY = (TotalColum - (UnitNo % TotalColum)) - 1;
                                else
                                    FindY = UnitNo % TotalColum;
                            }
                            else
                            {
                                if (((TotalRow - FindX - 1) % 2) == 0)
                                    FindY = UnitNo % TotalColum;
                                else
                                    FindY = (TotalColum - (UnitNo % TotalColum)) - 1;
                            }
                            break;
                        case EN_WORK_START_POINT.RIGHT_BOTTOM:
                            FindX = TotalRow - (UnitNo / TotalColum) - 1;
                            if ((UnitNo / (TotalRow * TotalColum) % 2) == 1 && (FindX % 2) != 0)
                            {
                                if (((TotalRow - FindX - 1) % 2) == 1)
                                    FindY = (TotalColum - (UnitNo % TotalColum)) - 1;
                                else
                                    FindY = UnitNo % TotalColum;
                            }
                            else
                            {
                                if (((TotalRow - FindX - 1) % 2) == 0)
                                    FindY = (TotalColum - (UnitNo % TotalColum)) - 1;
                                else
                                    FindY = UnitNo % TotalColum;
                            }
                            break;
                    }
                    #endregion
                    break;
            }
            #endregion

            #region
            FindGroupX = FindX / UnitCount.x;
            FindGroupY = FindY / UnitCount.y;
            FindX = FindX % UnitCount.x;
            FindY = FindY % UnitCount.y;

            if (FindGroupX < 0 || FindGroupY < 0 || FindX < 0 || FindY < 0)
            {
                FindGroupX = 0;
                FindGroupY = 0;
                FindX = 0;
                FindY = 0;
            }
            #endregion

            IPointXY FindGroupIndex, FindUnitIndex;

            FindGroupIndex.x = FindGroupX;
            FindGroupIndex.y = FindGroupY;
            FindUnitIndex.x = FindX;
            FindUnitIndex.y = FindY;

            GroupIndex = FindGroupIndex;
            UnitIndex = FindUnitIndex;
        }

        /// <summary>
        /// 해당 Unit No에 대한 Reference Unit Array Index를 반환한다.
        /// </summary>
        public static void GetUnitIndexByUnitNoOfReference(
            int UnitNo,
            IPointXY GroupCount,
            IPointXY UnitCount,
            out IPointXY GroupIndex,
            out IPointXY UnitIndex
            )
        {
            GetUnitIndexByUnitNo(UnitNo, GroupCount, UnitCount, EN_WORK_START_POINT.LEFT_TOP, EN_WORK_DIRECTION.HORIZONTAL_ONEWAY, out GroupIndex, out UnitIndex);
        }

        /// <summary>
        /// 해당 Unit Array Index(Total Unit Index)에 대한 Unit No를 반환한다.
        /// </summary>
        /// 2020.08.25. by shkim. [MOD] 홀수 인덱스 예외처리
        public static int GetUnitNoByUnitIndex(
            IPointXY UnitIndex,
            IPointXY GroupCount,
            IPointXY UnitCount,
            EN_WORK_START_POINT StartPos,
            EN_WORK_DIRECTION Direction,
            bool bIsVisionHead = false
            )
        {
            int FindColum = UnitIndex.y;
            int FindRow = UnitIndex.x;
            
            IPointXY TotalCount;
            TotalCount.x = GroupCount.x * UnitCount.x;
            TotalCount.y = GroupCount.y * UnitCount.y;
            
            // 2020.08.25. by shkim. [MOD] 홀수갯수 데이터 안읽히는 것 수정 (아래 코드 이유확인필요)
            if(bIsVisionHead)
            {
                if (FindRow >= TotalCount.x
                    || FindColum >= TotalCount.y)
                    return -1;
            }
//             if (FindRow >= TotalCount.x
//                 || FindColum >= TotalCount.y)
//                 return -1;

            #region
            switch (Direction)
            {
                case EN_WORK_DIRECTION.HORIZONTAL_ONEWAY:
                    switch(StartPos)
                    {
                        #region
                        case EN_WORK_START_POINT.LEFT_TOP:
                            return FindColum * TotalCount.x + FindRow;
                        case EN_WORK_START_POINT.LEFT_BOTTOM:
                            return ((TotalCount.y * TotalCount.x) - (FindColum * TotalCount.x) - TotalCount.x) + FindRow;
                        case EN_WORK_START_POINT.RIGHT_TOP:
                            return FindColum * TotalCount.x + (TotalCount.x - FindRow - 1);
                        case EN_WORK_START_POINT.RIGHT_BOTTOM:
                            return ((TotalCount.y * TotalCount.x) - (FindColum * TotalCount.x) - TotalCount.x) + (TotalCount.x - FindRow - 1);
                        #endregion
                    }
                    break;
                case EN_WORK_DIRECTION.HORIZONTAL_ZIGZAG:
                    switch (StartPos)
                    {
                        #region
                        case EN_WORK_START_POINT.LEFT_TOP:
                            if ((FindColum % 2) == 0)
                                return (FindColum * TotalCount.x + FindRow);
                            else return ((FindColum + 1) * TotalCount.x - FindRow - 1);
                        case EN_WORK_START_POINT.LEFT_BOTTOM:
                            if (((TotalCount.y - FindColum - 1) % 2) == 0)
                                return ((TotalCount.y * TotalCount.x) - (FindColum * TotalCount.x) - TotalCount.x) + FindRow;
                            else return ((TotalCount.y * TotalCount.x) - (FindColum * TotalCount.x) - TotalCount.x) + (TotalCount.x - FindRow - 1);
                        case EN_WORK_START_POINT.RIGHT_TOP:
                            if ((FindColum % 2) == 0)
                                return FindColum * TotalCount.x + (TotalCount.x - FindRow - 1);
                            else return (FindColum * TotalCount.x + FindRow);
                        case EN_WORK_START_POINT.RIGHT_BOTTOM:
                            if (((TotalCount.y - FindColum - 1) % 2) == 0)
                                return ((TotalCount.y * TotalCount.x) - (FindColum * TotalCount.x) - TotalCount.x) + (TotalCount.x - FindRow - 1);
                            else return ((TotalCount.y * TotalCount.x) - (FindColum * TotalCount.x) - TotalCount.x) + FindRow;
                        #endregion
                    }
                    break;
                case EN_WORK_DIRECTION.VERTICAL_ONEWAY:
                    switch (StartPos)
                    {
                        #region
                        case EN_WORK_START_POINT.LEFT_TOP:
                            return (FindRow * TotalCount.y + FindColum);
                        case EN_WORK_START_POINT.LEFT_BOTTOM:
                            return (FindRow * TotalCount.y) + (TotalCount.y - FindColum - 1);
                        case EN_WORK_START_POINT.RIGHT_TOP:
                            return (TotalCount.x * TotalCount.y) - (FindRow * TotalCount.y) - (TotalCount.y - FindColum);
                        case EN_WORK_START_POINT.RIGHT_BOTTOM:
                            return (TotalCount.x * TotalCount.y) - (FindRow * TotalCount.y) - FindColum - 1;
                        #endregion
                    }
                    break;
                case EN_WORK_DIRECTION.VERTICAL_ZIGZAG:
                    switch (StartPos)
                    {
                        #region
                        case EN_WORK_START_POINT.LEFT_TOP:
                            if ((FindRow % 2) == 0)
                                return (FindRow * TotalCount.y + FindColum);
                            else return ((FindRow + 1) * TotalCount.y - FindColum - 1);
                        case EN_WORK_START_POINT.LEFT_BOTTOM:
                            if ((FindRow % 2) == 0)
                                return (FindRow * TotalCount.y) + (TotalCount.y - FindColum - 1);
                            else return (FindRow * TotalCount.y) + FindColum;
                        case EN_WORK_START_POINT.RIGHT_TOP:
                            if (((TotalCount.x - FindRow - 1) % 2) == 0)
                                return (TotalCount.x * TotalCount.y) - (FindRow * TotalCount.y) - (TotalCount.y - FindColum);
                            else return (TotalCount.x * TotalCount.y) - (FindRow * TotalCount.y) - FindColum - 1;
                        case EN_WORK_START_POINT.RIGHT_BOTTOM:
                            if (((TotalCount.x - FindRow - 1) % 2) == 0)
                                return (TotalCount.x * TotalCount.y) - (FindRow * TotalCount.y) - FindColum - 1;
                            else return ((TotalCount.x * TotalCount.y) - (FindRow * TotalCount.y) - TotalCount.y) + FindColum;
                        #endregion
                    }
                    break;
            }
            #endregion
            return -1;
        }
        /// <summary>
        /// 해당 Unit Array Index에 대한 Reference Unit No를 반환한다.
        /// </summary>
        /// 2020.08.25. by shkim. [MOD] 홀수 인덱스 예외처리
        public static int GetUnitNoByUnitIndexOfReference(
            IPointXY UnitIndex,
            IPointXY GroupCount,
            IPointXY UnitCount,
            bool bIsVisionHead = false
            )
        {
            return GetUnitNoByUnitIndex(UnitIndex, GroupCount, UnitCount, EN_WORK_START_POINT.LEFT_TOP, EN_WORK_DIRECTION.HORIZONTAL_ONEWAY, bIsVisionHead);
        }
        /// <summary>
        /// Subject Reference에서 Target까지의 위치를 Align결과를 보상하여 반환한다.
        /// </summary>
        /// <param name="RefPos">Recipe상의 Subject Position</param>
        /// <param name="TargetPos">알고싶은 Position의 Recipe Position</param>
        /// <param name="Correction">Align 결과값 (= Offset = Recipe - Current)</param>
        /// <returns></returns>
        public static DPointXY GetWorkPosition(DPointXY RefPos, DPointXY TargetPos, DPointXYT Correction)
        {
            DPointXY dpReturn;
            DPointXY dpCorrection, dpCurrRef, dpCurrTgt;
            double dAngle;

            dpCorrection.x = Correction.x;
            dpCorrection.y = Correction.y;
            dAngle = Correction.t;

            dpCurrRef = RefPos + dpCorrection;
            dpCurrTgt = RefPos + TargetPos + dpCorrection;

            dpReturn = MathFormula.rotate(dAngle, dpCurrRef, dpCurrTgt);

            return dpReturn;
        }
		/// <summary>
		/// 해당 Index의 pitch를 계산한 위치를 반환한다.
		/// </summary>
		public static DPointXY GetPositionOfCalcPitch(DPointXY referencePosition, DPointXY groupPitch, DPointXY unitPitch, IPointXY groupIndex, IPointXY unitIndex)
		{
			DPointXY result = new DPointXY();
			result.x = (referencePosition.x + (groupPitch.x * groupIndex.x)) + (unitPitch.x * unitIndex.x);
			result.y = (referencePosition.y + (groupPitch.y * groupIndex.y)) + (unitPitch.y * unitIndex.y);
			return result;
		}
		/// <summary>
		/// 해당 unit no만큼 pitch를 계산한 위치를 반환한다.
		/// </summary>
		public static DPointXY GetPositionOfCalcPitch(int unitNo
			, IPointXY groupArray, IPointXY unitArray
			, DPointXY groupPitch, DPointXY unitPitch
			, DPointXY referencePosition)
		{
			IPointXY unitIndex, groupIndex;
			GetUnitIndexByUnitNoOfReference(unitNo, groupArray, unitArray, out groupIndex, out unitIndex);
			return GetPositionOfCalcPitch(referencePosition, groupPitch, unitPitch, groupIndex, unitIndex);
		}

		public static int GetOrderNo(
			int unitNo,
			IPointXY groupArray,
			IPointXY unitArray,
			EN_WORK_START_POINT enStartPoint,
			EN_WORK_DIRECTION enWorkDirection
			)
		{
			IPointXY ipGroupIndex, ipUnitIndex;
			GetUnitIndexByUnitNoOfReference(
							   unitNo, groupArray, unitArray
							   , out ipGroupIndex, out ipUnitIndex);
			ipUnitIndex.x = (ipGroupIndex.x * unitArray.x) + ipUnitIndex.x;
			ipUnitIndex.y = (ipGroupIndex.y * unitArray.y) + ipUnitIndex.y;
			int nOrderNo = GetUnitNoByUnitIndex(
				ipUnitIndex, groupArray, unitArray
				, enStartPoint, enWorkDirection
				);

			nOrderNo++;

			return nOrderNo;
		}
		public static int GetUnitNoByUnitIndexSimple(IPointXY index, IPointXY array)
		{
			return GetUnitNoByUnitIndexOfReference(index, new IPointXY(1, 1), array);
		}
		public static IPointXY GetUnitArrayByUnitNoSimple(int unitNo, IPointXY array)
		{
			IPointXY groupIndex, unitIndex;
			GetUnitIndexByUnitNoOfReference(unitNo, new IPointXY(1, 1), array, out groupIndex, out unitIndex);
			return unitIndex;
		}

        #region for Member
        /// <summary>
        /// 해당 Member가 가지고 있는 Unit의 List를 반환한다.
        /// </summary>
        public static List<int> GetMemberHaveUnitNoList(int memberNo
            , IPointXY memberCount
            , IPointXY memberArray
            , IPointXY groupArray
            , IPointXY ipGroupUnitArray
            , bool bIsVisionHead = false)
        {
			// 1. Member No -> Member Index
			IPointXY ipGroupIndex, ipMemberIndex;
			GetUnitIndexByUnitNoOfReference(memberNo, groupArray, memberCount, out ipGroupIndex, out ipMemberIndex);

			// 2. Member Index -> Unit Index
			IPointXY ipStartUnitIndex = (ipMemberIndex + (ipGroupIndex * memberCount)) * memberArray;

			// 2023.11.11 by junho [MOD] 식 버그 개선
			// 2021.03.09 by junho [ADD] Group을 가지고 있을 경우, 빈 행,열에 관한 오류 수정
			//IPointXY ipDel = memberArray - (ipGroupUnitArray % memberArray);
			//ipStartUnitIndex -= (ipGroupIndex * ipDel);

			//IPointXY ipDeleteIndex = (memberArray - (ipGroupUnitArray % memberArray)) * ipGroupIndex;
			IPointXY ipDeleteIndex = ipGroupUnitArray % memberArray;
			if (ipDeleteIndex.x == 0) ipDeleteIndex.x = memberArray.x;
			if (ipDeleteIndex.y == 0) ipDeleteIndex.y = memberArray.y;
			ipDeleteIndex = (memberArray - ipDeleteIndex) * ipGroupIndex;
			ipStartUnitIndex -= ipDeleteIndex;

			// 3. for() List Add
			int unitNo;
			IPointXY ipUnitIndex;
			List<int> listReturn = new List<int>();
			for (int OffsetX = 0; OffsetX < memberArray.x; OffsetX++)
			{
				for (int OffsetY = 0; OffsetY < memberArray.y; OffsetY++)
				{
					ipUnitIndex.x = ipStartUnitIndex.x + OffsetX;
					ipUnitIndex.y = ipStartUnitIndex.y + OffsetY;

					unitNo = GetUnitNoByUnitIndexOfReference(ipUnitIndex, groupArray, ipGroupUnitArray, bIsVisionHead);
					if (unitNo == -1)
						continue;

					listReturn.Add(unitNo);
				}
			}

			return listReturn;
        }
        /// <summary>
        /// 해당 Unit No가 어느 Member No에 속해 있는지 반환한다.
        /// </summary>
        public static int GetMemberNoOfUnitNo(int unitNo
            , IPointXY groupArray
            , IPointXY ipGroupUnitArray
            , IPointXY memberArray
            , IPointXY memberCount)
        {
			IPointXY ipGroupIndex, ipUnitIndex, ipMemberIndex;

			if (groupArray.x < 1 || groupArray.y < 1
				|| ipGroupUnitArray.x < 1 || ipGroupUnitArray.y < 1
				|| memberArray.x < 1 || memberArray.y < 1
				|| memberCount.x < 1 || memberCount.y < 1)
			{
				return -1;
			}

			// Unit No -> Unit Index
			WorkPosition.GetUnitIndexByUnitNoOfReference(unitNo, groupArray, ipGroupUnitArray, out ipGroupIndex, out ipUnitIndex);

			// Unit Index -> Member Index
			ipMemberIndex.x = (ipUnitIndex.x / memberArray.x) + (memberCount.x * ipGroupIndex.x);
			ipMemberIndex.y = (ipUnitIndex.y / memberArray.y) + (memberCount.y * ipGroupIndex.y);

			// Member Index -> Member No
			int nReturn = GetUnitNoByUnitIndexOfReference(ipMemberIndex, groupArray, memberCount);
			return nReturn;
        }

        /// <summary>
        /// Member내에서 Inner No가 어떤 Unit No인지 반환한다.
        /// </summary>
        public static int GetUnitNoByInMemberNo(
            int memberNo
            , int nInnerNo
            , IPointXY memberCount
            , IPointXY memberArray
            , IPointXY groupArray
            , IPointXY ipGroupUnitArray)
        {
            List<int> listUnit = WorkPosition.GetMemberHaveUnitNoList(memberNo, memberCount, memberArray, groupArray, ipGroupUnitArray);
            listUnit.Sort();
            if (listUnit.Count <= nInnerNo)
                return -1;

            return listUnit[nInnerNo];
        }
        #endregion
	}
}
