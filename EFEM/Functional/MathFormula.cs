using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;



namespace FrameOfSystem3
{
	class MathFormula
	{
		public static double x = 0;
		public static double b = 0;

		public static double degreeToRadian(double angle)
		{
			double rad = 0.0;
			rad = angle * (Math.PI / 180.0);

			return rad;
		}

		public static double radianToDegree(double rad)
		{
			double angle = 0.0;
			angle = rad * 180.0 / Math.PI;

			return angle;
		}


		/// <summary>
		/// 2022.05.26 by jhlee [ADD] 4Point Center
		/// pt1 = LT, pt2 = RB, pt3 = RT, pt4 = LB
		/// </summary>
		/// 출처 https://wjs7347.tistory.com/17
		public static DPointXY center4Point(DPointXY pt1, DPointXY pt2, DPointXY pt3, DPointXY pt4)
		{
			DPointXY dCenter;

			dCenter.x = (((pt1.x * pt2.y - pt1.y * pt2.x) * (pt3.x - pt4.x)) - ((pt1.x - pt2.x) * (pt3.x * pt4.y - pt3.y * pt4.x)))
						/ ((pt1.x - pt2.x) * (pt3.y - pt4.y) - (pt1.y - pt2.y) * (pt3.x - pt4.x));

			dCenter.y = (((pt1.x * pt2.y - pt1.y * pt2.x) * (pt3.y - pt4.y)) - ((pt1.y - pt2.y) * (pt3.x * pt4.y - pt3.y * pt4.x)))
						/ ((pt1.x - pt2.x) * (pt3.y - pt4.y) - (pt1.y - pt2.y) * (pt3.x - pt4.x));

			return dCenter;
		}

		public static DPointXYT centerPtoP(DPointXYT pt1, DPointXYT pt2)
		{
			DPointXYT dCenter;

			dCenter.x = (pt1.x + pt2.x) / 2.0;
			dCenter.y = (pt1.y + pt2.y) / 2.0;
			dCenter.t = 0;

			return dCenter;
		}

		public static double centerPtoP(double v1, double v2)
		{
			return (v1 + v2) / 2;
		}
		public static DPointXY centerPtoP(DPointXY pt1, DPointXY pt2)
		{
			DPointXY dCenter;

			dCenter.x = (pt1.x + pt2.x) / 2.0;
			dCenter.y = (pt1.y + pt2.y) / 2.0;

			return dCenter;
		}
		public static DPointXY distancePtoP(DPointXY pt1, DPointXY pt2, bool bAbsolute = true)
		{
			DPointXY dDistance;

			// 2021.06.11 by junho [MOD] MathFormula 개선
			//dDistance = pt1 - pt2;
			dDistance = pt2 - pt1;

			if (bAbsolute)
			{
				dDistance.x = Math.Abs(dDistance.x);
				dDistance.y = Math.Abs(dDistance.y);
			}

			return dDistance;
		}
		public static DPointXY distanceQuantifyPtoP(DPointXY pt1, DPointXY pt2)
		{
			DPointXY result;

			result.x = Math.Abs(Math.Max(pt1.x, pt2.x) - Math.Min(pt1.x, pt2.x));
			result.y = Math.Abs(Math.Max(pt1.y, pt2.y) - Math.Min(pt1.y, pt2.y));

			return result;
		}

		// 2021.06.15. by shkim. [확인완료]
		public static double distanceDiagonalPtoP(DPointXY pt1, DPointXY pt2)
		{
			double dDistance;

			dDistance = Math.Sqrt(Math.Pow((pt2.x - pt1.x), 2) + Math.Pow((pt2.y - pt1.y), 2));

			return dDistance;
		}
		
		/// <summary>
		/// pt1과 pt2를 잇는 선분에서 ratio에 해당하는 지점 반환
		/// </summary>
		public static DPointXY InterpolateLine(DPointXY pt1, DPointXY pt2, double ratio)
		{
			return new DPointXY(
				pt1.x + ((pt2.x - pt1.x) * ratio),
				pt1.y + ((pt2.y - pt1.y) * ratio));
		}
		/// <summary>
		/// pt1과 pt2를 잇는 선분에서 ratio에 해당하는 지점 반환
		/// </summary>
		public static DPointXY InterpolateLine(DPointXY pt1, DPointXY pt2, DPointXY ratio)
		{
			return new DPointXY(
				pt1.x + ((pt2.x - pt1.x) * ratio.x),
				pt1.y + ((pt2.y - pt1.y) * ratio.y));
		}

		// 2022.05.15. by wdw. [MOD] 반전 좌표계 추가
		// 2021.06.15. by shkim. [확인완료]
		// 2021.06.03. by shkim. [MOD] Atan -> Atan2 (Atan 예외발생)
		// Atan2 앵글 계산의 경우 무조건 Point 2에서 Point 1을 빼줘야 한다. (Point1에서 Point2 방향의 상대 각도)
		// 두 점의 앵글을 수평선 기준으로 구할 때는 pt1의 X 좌표 값이 무조건 왼쪽에 있어야한다.
		public static double anglePtoP(DPointXY pt1, DPointXY pt2, bool bReverse_x = false, bool bReverse_y = false)
		{
			double dAngle;

			if (bReverse_x)
			{
				pt1.x *= -1;
				pt2.x *= -1;
			}
			if (bReverse_y)
			{
				pt1.y *= -1;
				pt2.y *= -1;
			}

			//  Atan2 앵글 계산의 경우 무조건 Point 2에서 Point 1을 빼줘야 한다. (Point1에서 Point2 방향의 상대 각도)
			DPointXY dOffset = pt2 - pt1;

			dAngle = Math.Atan2(dOffset.y, dOffset.x);
			dAngle = radianToDegree(dAngle);
			return dAngle;
		}
		/// <summary>
		// 2022.05.15. by wdw. [MOD] 반전 좌표계 추가
		/// // 2021.06.15. by shkim. [확인완료]
		/// 회전변환 계산
		/// </summary>
		/// <param name="angle">적용시킬 Theta</param>
		/// <param name="ct">Center</param>
		/// <param name="pt1">Target</param>
		/// <returns></returns>
		public static DPointXY rotate(double angle, DPointXY ct, DPointXY pt1, bool bReverse_x = false, bool bReverse_y = false)
		{
			DPointXY pt2;

			double rad = degreeToRadian(angle);

			if (bReverse_x && bReverse_y)
			{
				pt2.x = ct.x - ((ct.x - pt1.x) * Math.Cos(rad) - (ct.y - pt1.y) * Math.Sin(rad));
				pt2.y = ct.y - ((ct.x - pt1.x) * Math.Sin(rad) + (ct.y - pt1.y) * Math.Cos(rad));

			}
			else if (bReverse_x)
			{
				pt2.x = ct.x - ((ct.x - pt1.x) * Math.Cos(rad) - (pt1.y - ct.y) * Math.Sin(rad));
				pt2.y = ct.y + ((ct.x - pt1.x) * Math.Sin(rad) + (pt1.y - ct.y) * Math.Cos(rad));

			}
			else if (bReverse_y)
			{
				pt2.x = ct.x + ((pt1.x - ct.x) * Math.Cos(rad) - (ct.y - pt1.y) * Math.Sin(rad));
				pt2.y = ct.y - ((pt1.x - ct.x) * Math.Sin(rad) + (ct.y - pt1.y) * Math.Cos(rad));

			}
			else
			{
				pt2.x = ct.x + ((pt1.x - ct.x) * Math.Cos(rad) - (pt1.y - ct.y) * Math.Sin(rad));
				pt2.y = ct.y + ((pt1.x - ct.x) * Math.Sin(rad) + (pt1.y - ct.y) * Math.Cos(rad));
			}

			return pt2;
		}

		// 2022.03.30 by jhchoo [ADD] 위치 회전 변환 : dPoint > dTransPoint
		public static DPointXY RotationalTransform(double dAngle, DPointXY dCenter, DPointXY dPoint)
		{
			DPointXY dTransPoint;

			double dRadian = degreeToRadian(dAngle);
			DPointXY dOffset = dPoint - dCenter;

			dTransPoint.x = dCenter.x + ((dOffset.x * Math.Cos(dRadian)) - (dOffset.y * Math.Sin(dRadian)));
			dTransPoint.y = dCenter.y + ((dOffset.x * Math.Sin(dRadian)) + (dOffset.y * Math.Cos(dRadian)));

			return dTransPoint;
		}

		public static DPointXY rotateDistance(double angle, DPointXY pt1, DPointXY pt2)
		{
			DPointXY modifyPt2 = new DPointXY(0.0, 0.0);
			DPointXY dDistance = new DPointXY(0.0, 0.0);

			double rad = degreeToRadian(angle);

			modifyPt2.x = pt1.x + (pt2.x - pt1.x) * Math.Cos(rad) - (pt2.y - pt1.y) * Math.Sin(rad);
			modifyPt2.y = pt1.y + (pt2.x - pt1.x) * Math.Sin(rad) + (pt2.y - pt1.y) * Math.Cos(rad);

			dDistance.x = Math.Abs(pt1.x - modifyPt2.x);
			dDistance.y = Math.Abs(pt1.y - modifyPt2.y);

			return dDistance;
		}

		public static void rotateCenter(double angle, DPointXY pt1, DPointXY pt2, out DPointXY pt_center)
		{
			double rad = degreeToRadian(angle);
			pt_center = new DPointXY();
			pt_center.x = 1 / (1 - 2 * Math.Cos(rad) + Math.Pow(Math.Cos(rad), 2) + Math.Pow(Math.Sin(rad), 2)) * (pt1.x * Math.Pow(Math.Sin(rad), 2) - pt2.y * Math.Sin(rad) - pt1.x * Math.Cos(rad) + pt1.y * Math.Sin(rad) + pt2.x + pt1.x * Math.Pow(Math.Cos(rad), 2) - Math.Cos(rad) * pt2.x);
			pt_center.y = -(pt1.x * Math.Sin(rad) - pt2.y + Math.Cos(rad) * pt2.y + pt1.y * Math.Cos(rad) - pt1.y * Math.Pow(Math.Cos(rad), 2) - pt1.y * Math.Pow(Math.Sin(rad), 2) - pt2.x * Math.Sin(rad)) /
				(1 - 2 * Math.Cos(rad) + Math.Pow(Math.Cos(rad), 2) + Math.Pow(Math.Sin(rad), 2));
		}

		// 2024.07.15 by junho [DEL] 확인결과 값이 제대로 나오지 않으므로 주석처리
		//public static void rotateCenter2(double angle, int side, DPointXY pt1, DPointXY pt2, out DPointXY pt_center)
		//{
		//	double dA = 0.0;
		//	double dB = 0.0;
		//	double dC = 0.0;

		//	double angle1 = 0.0;
		//	double angle2 = 0.0;
		//	double angle3 = 0.0;

		//	double resultX = 0.0;
		//	double resultY = 0.0;

		//	// 1. 두 점 사이의 거리를 구한다.
		//	dB = distanceDiagonalPtoP(pt1, pt2);

		//	// 2. 제2코사인 법칙을 이용해 회전 중심과 회전한 점 사이의 거리를 구한다.
		//	dA = Math.Sqrt((dB * dB) / (2 - 2 * (Math.Cos(angle * (Math.PI / 180)))));

		//	// 3. 이동된 y축 거리를 구한다.
		//	dC = Math.Abs(pt1.y - pt2.y);

		//	// 4. 이등변 삼각형의 밑각 (회전 중심과 2개의 점으로 만들어진 이등변 삼각형)
		//	angle2 = (180 - angle) / 2;

		//	//---------------------------------------------------------------------//
		//	// dC 와 이등변 삼각형 밑면 사이의 각
		//	// 역 코사인 Acos(X) = Atn(-X / Sqr(-X * X + 1)) + 2 * Atn(1)
		//	//### angle1 = ((Math.Atan(-(dC / dB) / Math.Sqrt(-(dC / dB) * (dC / dB) + 1))) * (180 / Math.PI)) + 2 * (Math.Atan(1) * (180 / Math.PI));

		//	// cos 역함수 사용 (바로 상단의 atan 이용한 값과 동일)
		//	angle1 = radianToDegree(Math.Acos(dC / dB));

		//	//---------------------------------------------------------------------//

		//	angle3 = (180 - angle1 - angle2);

		//	// 회전 중심과 회전한 좌표간의 X거리
		//	resultX = dA * Math.Sin(angle3 * (Math.PI / 180));

		//	// 회전 중심과 회전한 좌표간의 Y거리
		//	resultY = dA * Math.Cos(angle3 * (Math.PI / 180));

		//	DPointXY result1 = new DPointXY(0, 0);
		//	DPointXY result2 = new DPointXY(0, 0);
		//	DPointXY ct = new DPointXY(0, 0);
		//	double dTempAngle1, dTempAngle2, dTempAngle3;
		//	double dTempAngleResult1, dTempAngleResult2;

		//	switch (side)
		//	{
		//		case 1:
		//			// 1사분면
		//			result1.x = pt1.x - resultX;
		//			result1.y = pt1.y + resultY;
		//			result2.x = pt2.x - resultX;
		//			result2.y = pt2.y + resultY;
		//			break;
		//		case 2:
		//			// 2사분면
		//			result1.x = pt1.x + resultX;
		//			result1.y = pt1.y + resultY;
		//			result2.x = pt2.x + resultX;
		//			result2.y = pt2.y + resultY;
		//			break;
		//		case 3:
		//			// 3사분면
		//			result1.x = pt1.x + resultX;
		//			result1.y = pt1.y - resultY;
		//			result2.x = pt2.x + resultX;
		//			result2.y = pt2.y - resultY;
		//			break;
		//		case 4:
		//			// 4사분면
		//			result1.x = pt1.x - resultX;
		//			result1.y = pt1.y - resultY;
		//			result2.x = pt2.x - resultX;
		//			result2.y = pt2.y - resultY;
		//			break;
		//	}

		//	ct.x = (pt1.x + pt2.x) / 2;
		//	ct.y = (pt1.y + pt2.y) / 2;

		//	dTempAngle1 = anglePtoP(pt1, pt2);
		//	dTempAngle2 = anglePtoP(ct, result1);
		//	dTempAngle3 = anglePtoP(ct, result2);


		//	// 요아래부터 확인필요..
		//	if (dTempAngle1 > 180)
		//		dTempAngle1 = dTempAngle1 - 180;

		//	if (dTempAngle2 > 180)
		//		dTempAngle2 = dTempAngle2 - 180;

		//	if (dTempAngle3 > 180)
		//		dTempAngle3 = dTempAngle3 - 180;

		//	dTempAngleResult1 = dTempAngle1 - dTempAngle2 - 90;
		//	if (dTempAngleResult1 < 0)
		//		dTempAngleResult1 *= -1;

		//	dTempAngleResult2 = dTempAngle1 - dTempAngle3 - 90;
		//	if (dTempAngleResult2 < 0)
		//		dTempAngleResult2 *= -1;

		//	if (dTempAngleResult1 <= dTempAngleResult2)
		//	{
		//		pt_center = new DPointXY(result1);
		//	}
		//	else
		//	{
		//		pt_center = new DPointXY(result2);
		//	}
		//}

		/// <summary>
		/// Align 결과 XYT와 회전중심, 타겟위치를 받아 회전보상 및 XY 옵셋 적용 결과위치를 반환.
		/// </summary>
		public static DPointXY GetRotatePosition(DPointXYT dpAlignResult, DPointXY dpCenter, DPointXY dpTarget)
		{
			DPointXY dpRotate;
			DPointXY dpResult;
			double dAngle = dpAlignResult.t;

			// 회전보상
			dpRotate = MathFormula.rotate(dAngle, dpCenter, dpTarget);

			// XY 보상
			dpResult.x = dpRotate.x + dpAlignResult.x;
			dpResult.y = dpRotate.y + dpAlignResult.y;

			return dpResult;
		}

		/// <summary>
		/// 2020.06.24 by junho [ADD]
		/// 두 Double의 중앙값을 반환한다.
		/// 내부에서 High Low 구분함
		/// </summary>
		public static double GetMedianDtoD(double dValue1, double dValue2, bool bABS = false)
		{
			double dHigh, dLow, dResult;
			if (dValue1 == dValue2) return dValue1;
			if (dValue1 > dValue2)
			{
				dHigh = dValue1;
				dLow = dValue2;
			}
			else
			{
				dHigh = dValue2;
				dLow = dValue1;
			}

			if (dHigh >= 0 && dLow >= 0)
			{
				dResult = (dHigh - dLow) / 2;
			}
			else if (dHigh >= 0 && dLow < 0)
			{
				dResult = (dHigh + dLow) / 2;
			}
			else if (dHigh <= 0 && dLow < 0)
			{
				dResult = (dLow - dHigh) / 2;
			}
			else
			{
				// Console.WriteLine("[junho] MathFormula.GetCenterDtoD 확인");
				dResult = 0.0;
			}

			if (bABS) dResult = Math.Abs(dResult);

			return dResult;
		}
		/// <summary>
		/// 두 점의 1차 방정식 기울기 반환
		/// </summary>
		public static double segmentGradient(DPointXY pt1, DPointXY pt2)
		{
			return (pt1.y - pt2.y) / (pt1.x - pt2.x);
		}
		/// <summary>
		/// 두 점의 1차 방정식 y절편 반환
		/// </summary>
		public static double segmentIntersept(DPointXY pt1, DPointXY pt2)
		{
			return (-1 * pt1.x) * ((pt1.y - pt2.y) / (pt1.x - pt2.x)) + pt1.y;
		}
		/// <summary>
		/// 두 직선의 교차점을 반환한다.
		/// isSegment : 선분으로 범위를 제한
		/// </summary>
		public static bool crossPointByTwiceLine(DPointXY pointA1, DPointXY pointA2, DPointXY pointB1, DPointXY pointB2, out DPointXY result, bool isSegment = false)
		{
			double t;
			double s;
			double under = (pointB2.y - pointB1.y) * (pointA2.x - pointA1.x) - (pointB2.x - pointB1.x) * (pointA2.y - pointA1.y);
			if (under == 0) { result = new DPointXY(); return false; }		// 교차점x (평행 관계)

			double _t = (pointB2.x - pointB1.x) * (pointA1.y - pointB1.y) - (pointB2.y - pointB1.y) * (pointA1.x - pointB1.x);
			double _s = (pointA2.x - pointA1.x) * (pointA1.y - pointB1.y) - (pointA2.y - pointA1.y) * (pointA1.x - pointB1.x);
			if (_t == 0 && _s == 0)
			{
				// 2024.03.23 by junho [ADD] 애초에 교차점이 있는 상황이 들어오는지 확인하도록 개선 (하지만 평행하지는 않는)
				if(pointA1 == pointA2 || pointB1 == pointB2)
				{
					result = new DPointXY();
					return false;
				}

				int equalCount = 0;
				DPointXY crossPoint = new DPointXY();
				if (pointA1 == pointB1) { ++equalCount; crossPoint = pointA1; }
				if (pointA1 == pointB2) { ++equalCount; crossPoint = pointA1; }
				if (pointA2 == pointB1) { ++equalCount; crossPoint = pointA2; }
				if (pointA2 == pointB2) { ++equalCount; crossPoint = pointA2; }
				if (equalCount == 1)
				{
					result = crossPoint;
					return true;
				}

				result = new DPointXY();
				return false;
			}

			t = _t / under;
			s = _s / under;

			if (isSegment)	// 선분일 경우 범위를 제한
			{
				if (t < 0.0 || t > 1.0 || s < 0.0 || s > 1.0) { result = new DPointXY(); return false; }
			}


			result.x = pointA1.x + (t * (pointA2.x - pointA1.x));
			result.y = pointA1.y + (t * (pointA2.y - pointA1.y));

			return true;
		}
		/// <summary>
		/// 사각형의 무게중심 반환
		/// </summary>
		public static DPointXY GetCenterOfGravity(DPointXY pt1, DPointXY pt2, DPointXY pt3, DPointXY pt4)
		{
			// 각 pt의 사분면 확인
			double[,] points = new double[4, 2]
			{
				{pt1.x, pt1.y},
				{pt2.x, pt2.y},
				{pt3.x, pt3.y},
				{pt4.x, pt4.y}
			};

			// 가장 작은 x 좌표와 y 좌표를 갖는 점을 찾습니다.
			double minX = points[0, 0], minY = points[0, 1];
			int minIndex = 0;
			for (int i = 1; i < 4; i++)
			{
				if (points[i, 0] < minX)
				{
					minX = points[i, 0];
					minIndex = i;
				}
				if (points[i, 1] < minY)
				{
					minY = points[i, 1];
					minIndex = i;
				}
			}

			// 가장 큰 x 좌표와 y 좌표를 갖는 점을 찾습니다.
			double maxX = points[0, 0], maxY = points[0, 1];
			int maxIndex = 0;
			for (int i = 1; i < 4; i++)
			{
				if (points[i, 0] > maxX)
				{
					maxX = points[i, 0];
					maxIndex = i;
				}
				if (points[i, 1] > maxY)
				{
					maxY = points[i, 1];
					maxIndex = i;
				}
			}

			// 나머지 두 점을 찾습니다.
			int otherIndex1 = -1, otherIndex2 = -1;
			for (int i = 0; i < 4; i++)
			{
				if (i != minIndex && i != maxIndex)
				{
					if (otherIndex1 == -1)
					{
						otherIndex1 = i;
					}
					else
					{
						otherIndex2 = i;
					}
				}
			}

			int indexLT = -1, indexRB = -1;
			if (points[otherIndex1, 0] <= points[otherIndex2, 0]
				&& points[otherIndex1, 1] >= points[otherIndex2, 1])
			{
				indexLT = otherIndex1;
				indexRB = otherIndex2;
			}
			else
			{
				indexLT = otherIndex2;
				indexRB = otherIndex1;
			}

			DPointXY leftTop = new DPointXY(points[indexLT, 0], points[indexLT, 1]);
			DPointXY rightBottom = new DPointXY(points[indexRB, 0], points[indexRB, 1]);
			DPointXY leftBottom = new DPointXY(points[minIndex, 0], points[minIndex, 1]);
			DPointXY rightTop = new DPointXY(points[maxIndex, 0], points[maxIndex, 1]);

			// 사다리꼴을 이루는 두 삼각형의 무게중심과 면적을 계산
			double cx1 = (leftBottom.x + rightBottom.x + rightTop.x) / 3.0;
			double cy1 = (leftBottom.y + rightBottom.y + rightTop.y) / 3.0;
			double area1 = 0.5 * ((rightBottom.x - leftBottom.x) * (rightTop.y - leftBottom.y)
								 - (rightTop.x - leftBottom.x) * (rightBottom.y - leftBottom.y));

			double cx2 = (leftBottom.x + rightTop.x + leftTop.x) / 3.0;
			double cy2 = (leftBottom.y + rightTop.y + leftTop.y) / 3.0;
			double area2 = 0.5 * ((rightTop.x - leftBottom.x) * (leftTop.y - leftBottom.y)
								 - (leftTop.x - leftBottom.x) * (rightTop.y - leftBottom.y));

			// 전체 사다리꼴의 무게중심을 계산
			double cx = (cx1 * area1 + cx2 * area2) / (area1 + area2);
			double cy = (cy1 * area1 + cy2 * area2) / (area1 + area2);

			// 결과 출력
			return new DPointXY(cx, cy);
		}
		/// <summary>
		/// 삼각형의 무게중심 반환
		/// </summary>
		public static DPointXY GetCenterOfGravity(DPointXY pt1, DPointXY pt2, DPointXY pt3)
		{
			// 삼각형의 면적을 계산
			double area = 0.5 * ((pt2.x - pt1.x) * (pt3.y - pt1.y) - (pt3.x - pt1.x) * (pt2.y - pt1.y));

			// 삼각형의 무게중심을 계산
			double cx = (pt1.x + pt2.x + pt3.x) / 3.0;
			double cy = (pt1.y + pt2.y + pt3.y) / 3.0;

			return new DPointXY(cx, cy);
		}
		/// <summary>
		/// 초기 값에서 목적 값까지 범위의 지수함수 그래프에서 남은 시간에 해당하는 값을 반환한다.
		/// </summary>
		public static double GetExponentValue(double initValue, double destValue, int totalTime, int remainTime)
		{
			double result = (initValue - destValue) * GetExponent(totalTime, remainTime) + destValue;
			return result;
		}
		/// <summary>
		/// 시간 경과에 따른 지수함수를 보간값을 반환한다. (0~1사이의 값)
		/// </summary>
		public static double GetExponent(int totalTime, int remainTime)
		{
			double result = (Math.Pow(101, ((double)remainTime / (double)totalTime)) * 1.0) - 1;
			result *= 0.01;
			return result;
		}

		/// <summary>
		/// 두 원의 교점을 계산하는 함수
		/// </summary>
		public static bool GetCircleIntersections(DPointXY pt1, DPointXY pt2, double pt1r, double pt2r, out DPointXY[] result)
		{
			double d = Math.Sqrt(Math.Pow(pt2.x - pt1.x, 2) + Math.Pow(pt2.y - pt1.y, 2));

			// 두 원이 겹치지 않는 경우
			if (d > pt1r + pt2r || d < Math.Abs(pt1r - pt2r))
			{
				result = null;
				return false;
			}

			double a = (Math.Pow(pt1r, 2) - Math.Pow(pt2r, 2) + Math.Pow(d, 2)) / (2 * d);
			double h = Math.Sqrt(Math.Pow(pt1r, 2) - Math.Pow(a, 2));

			double x3 = pt1.x + a * (pt2.x - pt1.x) / d;
			double y3 = pt1.y + a * (pt2.y - pt1.y) / d;

			DPointXY intersectionpt1, intersectionpt2;
			intersectionpt1.x = x3 + h * (pt2.y - pt1.y) / d;
			intersectionpt1.y = y3 - h * (pt2.x - pt1.x) / d;

			intersectionpt2.x = x3 - h * (pt2.y - pt1.y) / d;
			intersectionpt2.y = y3 + h * (pt2.x - pt1.x) / d;

			result = new DPointXY[2] { intersectionpt1, intersectionpt2 };
			return true;
		}
		public static DPointXY GetConcentricityCenterPosition(DPointXY[] arPosition)
		{
			DPointXY dpPosition = new DPointXY();

			double d1 = (arPosition[1].x - arPosition[0].x) / (arPosition[1].y - arPosition[0].y);
			double d2 = (arPosition[2].x - arPosition[1].x) / (arPosition[2].y - arPosition[1].y);

			if (double.IsInfinity(d1))
				d1 = 1000000000000;
			if (double.IsInfinity(d2))
				d2 = 1000000000000;
			if (double.IsNaN(d1))
				d1 = 0.000000000001;
			if (double.IsNaN(d2))
				d2 = 0.000000000001;

			dpPosition.x = ((arPosition[2].y - arPosition[0].y) + (arPosition[1].x + arPosition[2].x) * d2 - (arPosition[0].x + arPosition[1].x) * d1) / (2 * (d2 - d1));
			dpPosition.y = -d1 * (dpPosition.x - (arPosition[0].x + arPosition[1].x) / 2) + (arPosition[0].y + arPosition[1].y) / 2;

			#region ppa code
			//             double p1, p2, ma, mb;
			//             p1 = standardPosY - concentricResultY[0];//(P2.Y - P1.Y);
			//             p2 = standardPosX - concentricResultX[0];//(P2.X - P1.X);
			//             ma = p1 / p2;
			//             p1 = concentricResultY[1] - standardPosY;//(P3.Y - P2.Y);
			//             p2 = concentricResultX[1] - standardPosX;//(P3.X - P2.X);
			//             mb = p1 / p2;
			// 
			//             if (double.IsInfinity(ma))
			//                 ma = 1000000000000;
			//             if (double.IsInfinity(mb))
			//                 mb = 1000000000000;
			//             if (double.IsNaN(ma))
			//                 ma = 0.000000000001;
			//             if (double.IsNaN(mb))
			//                 mb = 0.000000000001;
			// 
			//             concentricX = (ma * mb * (concentricResultY[0] - concentricResultY[1]) +
			//                                     mb * (concentricResultX[0] + standardPosX) -
			//                                     ma * (standardPosX + concentricResultX[1])) / (2 * (mb - ma));
			// 
			//             concentricY = (-1 * (concentricX - (concentricResultX[0] + standardPosX) / 2) / ma) +
			//                                    ((concentricResultY[0] + standardPosY) / 2);
			// 
			//             radius = Math.Sqrt(Math.Pow(concentricX, 2) + Math.Pow(concentricY, 2));
			//             concentricT = Math.Atan2(-concentricY, -concentricX) * 180 / Math.PI;
			#endregion

			return dpPosition;
		}

		// 23.08.23. by wdw [ADD] homography 보간
		private static void homography_from_4pt(DPointXY pt1, DPointXY pt2, DPointXY pt3, DPointXY pt4, ref double[] h)
		{
			h = new double[8];

			double t1 = pt1.x; double t2 = pt3.x; double t4 = pt2.y; double t5 = t1 * t2 * t4;
			double t6 = pt4.y; double t7 = t1 * t6; double t8 = t2 * t7; double t9 = pt3.y;
			double t10 = t1 * t9; double t11 = pt2.x; double t14 = pt1.y; double t15 = pt4.x;
			double t16 = t14 * t15; double t18 = t16 * t11; double t20 = t15 * t11 * t9;
			double t21 = t15 * t4; double t24 = t15 * t9; double t25 = t2 * t4;
			double t26 = t6 * t2; double t27 = t6 * t11; double t28 = t9 * t11;
			double t30 = 0.1e1 / (-t24 + t21 - t25 + t26 - t27 + t28);
			double t32 = t1 * t15; double t35 = t14 * t11; double t41 = t4 * t1;
			double t42 = t6 * t41; double t43 = t14 * t2; double t46 = t16 * t9;
			double t48 = t14 * t9 * t11; double t51 = t4 * t6 * t2; double t55 = t6 * t14;

			h[0] = -(-t5 + t8 + t10 * t11 - t11 * t7 - t16 * t2 + t18 - t20 + t21 * t2) * t30;
			h[1] = (t5 - t8 - t32 * t4 + t32 * t9 + t18 - t2 * t35 + t27 * t2 - t20) * t30;
			h[2] = t1;
			h[3] = (-t9 * t7 + t42 + t43 * t4 - t16 * t4 + t46 - t48 + t27 * t9 - t51) * t30;
			h[4] = (-t42 + t41 * t9 - t55 * t2 + t46 - t48 + t55 * t11 + t51 - t21 * t9) * t30;
			h[5] = t14;
			h[6] = (-t10 + t41 + t43 - t35 + t24 - t21 - t26 + t27) * t30;
			h[7] = (-t7 + t10 + t16 - t43 + t27 - t28 - t21 + t25) * t30;
		}

		private static void homography_from_4corresp(
		   DPointXY pt1, DPointXY pt2, DPointXY pt3, DPointXY pt4,
		   DPointXY matched_pt1, DPointXY matched_pt2, DPointXY matched_pt3, DPointXY matched_pt4, ref double[] H)
		{
			H = new double[9];

			double[] Hr = new double[8];
			double[] Hl = new double[8];

			homography_from_4pt(pt1, pt2, pt3, pt4, ref Hr);
			homography_from_4pt(matched_pt1, matched_pt2, matched_pt3, matched_pt4, ref Hl);

			// the following code computes R = Hl * inverse Hr
			double t2 = Hr[4] - Hr[7] * Hr[5];
			double t4 = Hr[0] * Hr[4];
			double t5 = Hr[0] * Hr[5];
			double t7 = Hr[3] * Hr[1];
			double t8 = Hr[2] * Hr[3];
			double t10 = Hr[1] * Hr[6];
			double t12 = Hr[2] * Hr[6];
			double t15 = 1 / (t4 - t5 * Hr[7] - t7 + t8 * Hr[7] + t10 * Hr[5] - t12 * Hr[4]);
			double t18 = -Hr[3] + Hr[5] * Hr[6];
			double t23 = -Hr[3] * Hr[7] + Hr[4] * Hr[6];
			double t28 = -Hr[1] + Hr[2] * Hr[7];
			double t31 = Hr[0] - t12;
			double t35 = Hr[0] * Hr[7] - t10;
			double t41 = -Hr[1] * Hr[5] + Hr[2] * Hr[4];
			double t44 = t5 - t8;
			double t47 = t4 - t7;
			double t48 = t2 * t15;
			double t49 = t28 * t15;
			double t50 = t41 * t15;

			H[0] = Hl[0] * t48 + Hl[1] * (t18 * t15) - Hl[2] * (t23 * t15);
			H[1] = Hl[0] * t49 + Hl[1] * (t31 * t15) - Hl[2] * (t35 * t15);
			H[2] = -Hl[0] * t50 - Hl[1] * (t44 * t15) + Hl[2] * (t47 * t15);
			H[3] = Hl[3] * t48 + Hl[4] * (t18 * t15) - Hl[5] * (t23 * t15);
			H[4] = Hl[3] * t49 + Hl[4] * (t31 * t15) - Hl[5] * (t35 * t15);
			H[5] = -Hl[3] * t50 - Hl[4] * (t44 * t15) + Hl[5] * (t47 * t15);
			H[6] = Hl[6] * t48 + Hl[7] * (t18 * t15) - t23 * t15;
			H[7] = Hl[6] * t49 + Hl[7] * (t31 * t15) - t35 * t15;
			H[8] = -Hl[6] * t50 - Hl[7] * (t44 * t15) + t47 * t15;
		}

		public static DPointXY homography_Point(DPointXY target_pt,
		   DPointXY pt1, DPointXY pt2, DPointXY pt3, DPointXY pt4,
		   DPointXY matched_pt1, DPointXY matched_pt2, DPointXY matched_pt3, DPointXY matched_pt4)
		{
			DPointXY dpReturn = new DPointXY();

			double[] H = new double[9];

			homography_from_4corresp(pt1, pt2, pt3, pt4, matched_pt1, matched_pt2, matched_pt3, matched_pt4, ref H);

			dpReturn.x = (H[0] * target_pt.x + H[1] * target_pt.y + H[2]) / (H[6] * target_pt.x + H[7] * target_pt.y + H[8]);
			dpReturn.y = (H[3] * target_pt.x + H[4] * target_pt.y + H[5]) / (H[6] * target_pt.x + H[7] * target_pt.y + H[8]);
			return dpReturn;
		}

		// 23.08.23. by wdw [ADD]  Bilinear 보간

		public static double GetBilinearTransferValue(DPointXY Target_pt, DPointXY pt1, double Value1, DPointXY pt2, double Value2, DPointXY pt3, double Value3, DPointXY pt4, double Value4)
		{
			double u = (Target_pt.x - pt1.x) / (pt2.x - pt1.x);
			double v = (Target_pt.y - pt1.y) / (pt3.y - pt1.y);

			return (1 - u) * (1 - v) * Value1 + u * (1 - v) * Value2 + (1 - u) * v * Value3 + u * v * Value4;
		}

		// 23.08.23. by wdw [ADD]  점이 다각형 내부에 존재하는지 판단.
		public static bool IsPointInPolygon(DPointXY Target_pt, List<DPointXY> sourceList)
		{
			int nCount = 0;
			int j = sourceList.Count - 1;

			for (int i = 0; i < sourceList.Count; i++)
			{
				if (sourceList[i].y <= Target_pt.y && sourceList[j].y >= Target_pt.y
					|| sourceList[j].y <= Target_pt.y && sourceList[i].y >= Target_pt.y)
				{
					double value = sourceList[i].x + (Target_pt.y - sourceList[i].y) /
					   (sourceList[j].y - sourceList[i].y) * (sourceList[j].x - sourceList[i].x);

					if (value == Target_pt.x)
					{
						return true;
					}
					else if (value > Target_pt.x)
					{
						nCount++;
					}
				}

				j = i;
			}

			return nCount % 2 == 1;
		}

		/// <summary>
		/// 2024.05.16. by shkim. [ADD] 선형보간공식.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="aValue"></param>
		/// <param name="bValue"></param>
		/// <returns></returns>
        public static bool TryLinearInterpolate(double[] a, double[] b, double aValue, out double bValue, bool isIncreasing = true)
        {
            bValue = 0.0;

            if (a.Length != b.Length || a.Length < 2)
            {
                return false;
            }

            // Find the interval [a[i], a[i+1]] where aValue fits
            for (int i = 0; i < a.Length - 1; i++)
            {
				bool checkCondition = false;

                if(isIncreasing)
				{
                    checkCondition = aValue >= a[i] && aValue <= a[i + 1];
                }
				else
				{
                    checkCondition = aValue <= a[i] && aValue >= a[i + 1];
                }

                // if (aValue >= a[i] && aValue <= a[i + 1])
				if(checkCondition)
                {
                    // Calculate the interpolation factor t
                    double t = (aValue - a[i]) / (a[i + 1] - a[i]);

                    // Calculate the interpolated value
                    bValue = (1 - t) * b[i] + t * b[i + 1];

                    return true;
                }
            }

            // If aValue is not in the range of array a
            return false;
        }
    }
}
