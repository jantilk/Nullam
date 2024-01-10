const isValidEstonianIdCode = (idCode: string): boolean => {
  if (idCode.length !== 11 || !idCode.split('').every(char => !isNaN(parseInt(char)))) {
    return false;
  }

  const multipliers1: number[] = [1, 2, 3, 4, 5, 6, 7, 8, 9, 1];
  const multipliers2: number[] = [3, 4, 5, 6, 7, 8, 9, 1, 2, 3];

  let checksum: number = calculateSum(idCode, multipliers1) % 11;
  if (checksum < 10) {
    return checksum === parseInt(idCode.charAt(10));
  }

  checksum = calculateSum(idCode, multipliers2) % 11;
  if (checksum === 10) {
    checksum = 0;
  }
  return checksum === parseInt(idCode.charAt(10));
};

const calculateSum = (idCode: string, multipliers: number[]): number => {
  return idCode.substring(0, 10).split('').reduce((sum, current, index) => {
    return sum + parseInt(current) * multipliers[index];
  }, 0);
};

const utils = {
  isValidEstonianIdCode
}

export default utils;
