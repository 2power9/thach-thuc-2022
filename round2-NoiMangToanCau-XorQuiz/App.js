import React, { useState, useEffect } from 'react';
import {
  SafeAreaView,
  ScrollView,
  StatusBar,
  StyleSheet,
  Text,
  useColorScheme,
  View,
  TouchableOpacity,
  FlatList,
  Dimensions,
} from 'react-native';

const Mode = {
  Idle: 'IDLE',
  Starting: 'STARTING',
  Key: 'KEY',
};

const StatusTitle = {
  [Mode.Idle]: 'Idle',
  [Mode.Starting]: 'Starting',
  [Mode.Key]: 'Key',
};

const FunctionButtonTitle = {
  [Mode.Idle]: 'Start',
  [Mode.Starting]: 'Stop',
  [Mode.Key]: 'Random',
}

const { width, height } = Dimensions.get('window');

const App = () => {
  const [mode, setMode] = useState(Mode.Idle);
  const [key, setKey] = useState("");
  const [status, setStatus] = useState(null);
  const [problem, setProblem] = useState("");
  const [solution, setSolution] = useState("");
  const [score, setScore] = useState(0);
  const [numProblems, setNumProblems] = useState(0);
  const [startingTime, setStartingTime] = useState(null);

  useEffect(() => {
    if (mode === Mode.Starting) {
      setStartingTime(new Date());
      setNumProblems(0);
    } else {
      setScore(0);
    }
  }, [mode]);

  useEffect(() => {
    if (key.length === 2) {
      setMode(Mode.Idle);
      setProblem("");
      setSolution("");
    }
  }, [key]);

  useEffect(() => {
    if (solution.length === 4) {
      const isCorrect = checkSolution(problem, solution);
      if (isCorrect) {
        setScore(score => score + 1);
        setStatus("Correct");
      } else {
        setStatus("Incorrect");
      }
      generateProblem();
      setSolution("");
    }
  }, [solution]);

  const checkSolution = (problem, solution) => {
    const solutionArray = solution.split("").map(num => parseInt(num, 16));
    const problemArray = problem.split("").map(num => parseInt(num, 16));
    const keyArray = [...key.split(""), ...key.split("")].map(num => parseInt(num, 16));
    let isCorrect = true;
    for (let i = 0; i < 4; i++) {
      if ((problemArray[i] ^ keyArray[i]) !== solutionArray[i]) {
        isCorrect = false;
      }
    }
    return isCorrect;
  }

  const generateProblem = () => {
    setNumProblems(numProblems => numProblems + 1);
    setProblem(Math.random().toString(16).substring(2, 6));
  }

  const generateKey = () => {
    setKey(Math.random().toString(16).substring(2, 4));
  }

  const onPressNumber = (number) => {
    setStatus(null);
    if (mode === Mode.Starting) {
      setSolution(solution => solution.concat(number.toString(16)));
    } else if (mode === Mode.Key) {
      setKey(key => key.concat(number.toString(16)));
    }
  }

  const onPressBackspace = () => {
    setStatus(null);
    if (mode === Mode.Starting) {
      setSolution(solution => solution.slice(0, -1));
    } else if (mode === Mode.Key) {
      setKey(key => key.slice(0, -1));
    }
  }

  const onPressFunctionButton = () => {
    setStatus(null);
    if (mode === Mode.Idle) {
      if (key === "") {
        generateKey();
      } else {
        generateProblem();
        setMode(Mode.Starting);
      }
    } else if (mode === Mode.Starting) {
      setProblem("");
      setSolution("");
      setKey("");
      setMode(Mode.Idle);
      setStatus(numProblems ? `${Math.trunc(score / numProblems * 100)}% - ${((new Date() - startingTime) / (4 * numProblems * 1000)).toFixed(2)}s` : null);
    } else if (mode === Mode.Key) {
      generateKey();
      setMode(Mode.Idle);
    }
  }

  const onPressKeyButton = () => {
    setStatus(null);
    setMode(Mode.Key);
    setKey("");
  }

  const renderItem = ({ item }) => {

    return (
      <TouchableOpacity style={styles.numberButton} onPress={() => onPressNumber(item)}>
        <Text style={styles.text}>
          {item.toString(16)}
        </Text>
      </TouchableOpacity>
    );
  }

  const getSolutionTitle = (solution) => {
    return solution.length >= 4 ? solution.substring(0, 4) : solution + (new Array(4 - solution.length)).fill("_").join("");
  }

  const getKeyTitle = (key) => {
    return key.length >= 2 ? key.substring(0, 2) : key + (new Array(2 - key.length)).fill("_").join("");
  }

  return (
    <SafeAreaView style={{ backgroundColor: '#90e0ef' }}>
      <StatusBar barStyle={'dark-content'} />
      <View
        contentInsetAdjustmentBehavior="automatic"
        style={styles.container}
      >
        <View style={{ flex: 1 }}>
          <View style={styles.header}>
            <TouchableOpacity style={styles.button} onPress={onPressKeyButton}>
              <Text style={styles.textSmall}>
                {getKeyTitle(key)}
              </Text>
            </TouchableOpacity>
            <View style={styles.button}>
              <Text style={styles.textSmall}>
                {status || StatusTitle[mode]}
              </Text>
            </View>
          </View>
          <View style={styles.button}>
            <Text style={styles.textLarge}>
              {getSolutionTitle(problem)}
            </Text>
          </View>
          <View style={styles.button}>
            <Text style={styles.textLarge}>
              {getSolutionTitle(solution)}
            </Text>
          </View>
        </View>
        <View style={{ height: width - 10 }}>
          <FlatList
            data={[...Array(16).keys()]}
            renderItem={renderItem}
            keyExtractor={(item, index) => index.toString()}
            scrollEnabled={false}
            numColumns={4}
          />
        </View>
        <View style={styles.footer}>
          <TouchableOpacity style={styles.functionButton} onPress={onPressFunctionButton}>
            <Text style={styles.textBold}>
              {FunctionButtonTitle[mode]}
            </Text>
          </TouchableOpacity>
          <TouchableOpacity style={styles.numberButton} onPress={onPressBackspace}>
            <Text style={styles.text}>
              {"<"}
            </Text>
          </TouchableOpacity>
        </View>
      </View>
    </SafeAreaView>
  );
};

const styles = StyleSheet.create({
  container: {
    backgroundColor: '#fff',
    height: '100%',
    padding: 5,
    paddingTop: 0,
    backgroundColor: '#90e0ef',
  },
  header: {
    flexDirection: 'row',
    flex: 0.7,
  },
  button: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
    margin: 5,
    borderRadius: 36,
    backgroundColor: '#caf0f8',
  },
  numberButton: {
    alignItems: 'center',
    justifyContent: 'center',
    margin: 5,
    borderRadius: 30,
    backgroundColor: '#caf0f8',
    width: (width - 50) / 4,
    height: (width - 50) / 4,
  },
  functionButton: {
    alignItems: 'center',
    justifyContent: 'center',
    margin: 5,
    borderRadius: 30,
    backgroundColor: '#caf0f8',
    height: (width - 50) / 4,
    flex: 1,
  },
  text: {
    fontSize: 30,
    color: '#03045e',
  },
  textBold: {
    fontSize: 30,
    color: '#03045e',
    fontWeight: 'bold',
  },
  textSmall: {
    fontSize: 20,
    color: '#03045e',
  },
  textLarge: {
    fontSize: 44,
    marginLeft: 30,
    letterSpacing: 30,
    color: '#03045e',
  },
  footer: {
    flexDirection: 'row',
    height: (width - 50) / 4,
  },
});

export default App;
