using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

/**
 * Created by betmansmall on 18.02.2016.
 */
public class WaveAlgorithm {
    private bool CIRCLET8 = true;

    // private List<int> mapWithSteps;
//    private Array<Integer> outMap;
    private int sizeX, sizeY;
    private int exitPointX, exitPointY;
    // private TiledMapTileLayer layer;
    public Cell[,] field;
    private bool found;
    // private Thread thread;

    public WaveAlgorithm(int sizeX, int sizeY, int exitPointX, int exitPointY, Cell[,] field) {
        Debug.Log("WaveAlgorithm::WaveAlgorithm(); -- Start!");
        this.sizeX = sizeX;
        this.sizeY = sizeY;
        this.exitPointX = exitPointX;
        this.exitPointY = exitPointY;
        this.field = field;
        // this.layer = layer;
        // this.mapWithSteps = new List<int>(sizeX*sizeY);
//        this.outMap = new Array<Integer>(sizeX*sizeY);
        clearStepsOnWaveAlgorithm();
        searh();
        Debug.Log("WaveAlgorithm::WaveAlgorithm(); -- End!");
    }

    public bool isFound() {
        return found;
    }

    public int getNumStep(int x, int y) {
        if(found) {
            if (x >= 0 && x < sizeX) {
                if (y >= 0 && y < sizeY) {
                    Debug.Log("WaveAlgorithm::getNumStep(); -- getStepCellWithOutIfs x:" + x + " y:" + y + " empty:" + cellIsEmpty(x, y));
                    // if (cellIsEmpty(x, y)) {
                        return getStepCellWithOutIfs(x, y);
                    // }
                    // Debug.Log("WaveAlgorithm::getNumStep(); -- return 0!");
                    // return 0;
                }
            }
        }
        return -1;
    }

    public void searh() {
        Debug.Log("WaveAlgorithm::searh(); -- Searh start!");
        researh(exitPointX, exitPointY);
        Debug.Log("WaveAlgorithm::searh(); -- Searh stop!");
    }

    public void researh(int x, int y) {
        clearStepsOnWaveAlgorithm();

        setNumOfCell(x, y, 1);
        Thread t = new Thread(Run);
        t.Start();
    }
    
    public void Run() {
        Debug.Log("WaveAlgorithm::Run(); -- Thread Start! " + Thread.CurrentThread);
        waveStep(exitPointX, exitPointX, 1);
        found = true;
        Debug.Log("WaveAlgorithm::Run(); -- Thread END! " + Thread.CurrentThread);
    }

    private void waveStep(int x, int y, int step) {
//        Debug.Log("WaveAlgorithm::waveStep(); -- heap:" + Gdx.app.getJavaHeap() + " Step:" + step);
//        if(Thread.currentThread().isInterrupted()) {
//            Debug.Log("WaveAlgorithm::waveStep()-isInterrupted; -- Thread work:" + Thread.currentThread().toString() + " Step:" + step);
//            return;
//        }
        //------------3*3----------------
        if(CIRCLET8) {
            bool[,] mass = new bool[3,3];
            int nextStep = step + 1;

            for (int tmpY = -1; tmpY < 2; tmpY++)
                for (int tmpX = -1; tmpX < 2; tmpX++)
                    mass[tmpX + 1,tmpY + 1] = setNumOfCell(x + tmpX, y + tmpY, nextStep);

            for (int tmpY = -1; tmpY < 2; tmpY++)
                for (int tmpX = -1; tmpX < 2; tmpX++)
                    if (mass[tmpX + 1,tmpY + 1])
                        waveStep(x + tmpX, y + tmpY, nextStep);
        } else {
            //------------2*2-----------------
            bool[] mass = new bool[4];
            int nextStep = step + 1;
            int x1 = x - 1, x2 = x, x3 = x + 1;
            int y1 = y - 1, y2 = y, y3 = y + 1;

            mass[0] = setNumOfCell(x1, y2, nextStep);
            mass[1] = setNumOfCell(x2, y1, nextStep);
            mass[2] = setNumOfCell(x2, y3, nextStep);
            mass[3] = setNumOfCell(x3, y2, nextStep);

            if (mass[0])
                waveStep(x1, y2, nextStep);
            if (mass[1])
                waveStep(x2, y1, nextStep);
            if (mass[2])
                waveStep(x2, y3, nextStep);
            if (mass[3])
                waveStep(x3, y2, nextStep);
        }
    }

    private bool setNumOfCell(int x, int y, int step) {
        if(x >= 0 && x < sizeX) {
            if(y >= 0 && y < sizeY) {
                if(cellIsEmpty(x, y)) {
                    if(getStepCellWithOutIfs(x, y) > step || getStepCellWithOutIfs(x, y) == 0) {
                        setStepCell(x, y, step);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private int getStepCellWithOutIfs(int x, int y) {
        return field[x, y].numberByWaveAlgorithm;
    }

    private void setStepCell(int x, int y, int step) {
//        Gdx.app.log("WaveAlgorithm::setStepCell()", "-- x:" + x + " y:" + y + " step:" + step + " sum:" + (sizeX*y + x));
        field[x, y].numberByWaveAlgorithm = step;
    }

    private void clearStepsOnWaveAlgorithm() {
        Debug.Log("WaveAlgorithm::clearStepsOnWaveAlgorithm(); -- Start! sizeX:" + sizeX + " sizeY:" + sizeY);
        found = false;
        for (int tmpX = 0; tmpX < sizeX; tmpX++) {
            for (int tmpY = 0; tmpY < sizeY; tmpY++) {
                // mapWithSteps.Add(0); // КОСТЫЛЬ МАТЬ ЕГО!!!!!
                // Debug.Log("WaveAlgorithm::clearStepsOnWaveAlgorithm(); -- tmpX:" + tmpX + " sizeX * tmpY:" + sizeX * tmpY + " tmpY:" + tmpY);
                field[tmpX, tmpY].numberByWaveAlgorithm = 0;
            }
        }
        Debug.Log("WaveAlgorithm::clearStepsOnWaveAlgorithm(); -- End!");
    }

    private bool cellIsEmpty(int x, int y) {
        Cell cell = field[x, y];
        // Debug.Log("WaveAlgorithm::cellIsEmpty(); -- x:" + x + " y:" + y + " cell:" + (cell!=null));
        if (cell != null && !cell.isEmpty()) {
            return false;
        } else {
            return true;
        }
    }
}
